using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace XmlDocUtil
{
    class XmlToMd
    {
        static void Main(string[] args)
        {
            if (
                (args.Length == 0) ||
                (!File.Exists(args[0]))
               )
            {
                return;
            }
            
            XmlToMd app = new XmlToMd();
            string md = app.ToMarkdown(args[0]);

            if (args.Length == 2)
            {
                File.WriteAllText(args[1], md);
            }
            else
            {
                File.WriteAllText(Path.GetFileNameWithoutExtension(args[0]) + ".md", md);
            }
        }

        private Dictionary<string, string> context;

        public XmlToMd()
        {
            context = new Dictionary<string, string>();
            context["lastNode"] = null;
        }

        public string ToMarkdown(string filePath)
        {
            var xdoc = XDocument.Load(filePath);
            var sw = new StringWriter();
            this.ToMarkdown(sw, xdoc.Root);
            return sw.ToString();
        }

        private void ToMarkdown(StringWriter sw, XElement root)
        {
            if (root.Name != "param" && context["lastNode"] == "param")
            {
                sw.WriteLine();
            }
            if (root.Name == "doc")
            {
                foreach (var node in root.Nodes())
                {
                    var elem = (XElement)node;
                    if (elem.Name == "assembly")
                    {
                        context["assembly"] = elem.Element("name").Value;
                        sw.WriteLine("\n# {0}\n", context["assembly"]);
                    }
                    else if (elem.Name == "members")
                    {
                        ToMarkdown(sw, elem);
                    }
                }
            }
            else if (root.Name == "members")
            {
                // Sorts by member name to regroup them all properly.
                var members = new List<XElement>(root.Elements("member"));
                members.Sort((a, b) =>
                    a.Attribute(XName.Get("name")).Value.Substring(2).CompareTo(
                    b.Attribute(XName.Get("name")).Value.Substring(2)));

                foreach (var member in members)
                {
                    ToMarkdown(sw, member);
                }
            }
            else if (root.Name == "member")
            {
                var memberName = root.Attribute(XName.Get("name")).Value;
                char memberType = memberName[0];

                if (memberType == 'P')
                {
                    // Do not published Properies..
                    return;
                }
                if (memberType == 'M')
                {
                    memberName = RearrangeParametersInContext(root);
                }
                if (memberType == 'T')
                {
                    string shortMemberName, assemblyRemove;
                    if (memberName.Contains(":" + context["assembly"] + "."))
                    {
                        assemblyRemove = String.Format("T:{0}.", context["assembly"]);
                        shortMemberName = memberName.Replace(assemblyRemove, "");
                    }
                    else
                    {
                        shortMemberName = memberName.Replace("T:", "");
                    }
                    sw.WriteLine("\n## Class: {0}\n", shortMemberName);
                    context["typeName"] = shortMemberName;
                }
                else
                {
                    string shortMemberName = memberName.Replace("P:" + context["assembly"], "").Replace(context["typeName"] + ".", "");
                    if (shortMemberName.StartsWith("#ctor"))
                    {
                        shortMemberName = shortMemberName.Replace("#ctor", "Constructor");
                    }
                    sw.WriteLine("\n### {0}\n", shortMemberName);
                }

                foreach (var node in root.Nodes())
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        ToMarkdown(sw, (XElement)node);
                    }
                }
            }
            else if (root.Name == "summary")
            {
                // string summary = Regex.Replace(root.Value, "\\s+", " ", RegexOptions.Multiline);
                // sw.WriteLine("{0}\n", summary.Trim());
                foreach (string line in root.Value.Trim().Split('\n'))
                {
                    sw.WriteLine("{0}", line.Trim());
                }
                sw.WriteLine("\n");
            }
            else if (root.Name == "code")
            {
                sw.WriteLine("##### Code:\n\n```csharp\n\n{0}\n```\n", root.Value.Trim());
            }
            else if ((root.Name == "see") || (root.Name == "seealso"))
            {
                string cref = NormalizeCrefMethod(root.ToString());
                if (!string.IsNullOrWhiteSpace(cref))
                {
                    sw.WriteLine("> See also: {0}\n", cref);
                }
            }
            else if (root.Name == "example")
            {
                string example = root.Value.Trim();
                if (example.Contains("<code>"))
                {
                    example = example.Replace("<code>", "\n\n").Replace("</code>", "\n\n");
                }
                sw.WriteLine("##### Example:\n\n```\n{0}\n```\n", example);
            }
            else if (root.Name == "param")
            {
                if (context["lastNode"] != "param")
                {
                    sw.WriteLine("| Name | Description |");
                    sw.WriteLine("| ---- | ----------- |");
                }

                string paramName = root.Attribute(XName.Get("name")).Value;
                if (context.ContainsKey(paramName))
                {
                    sw.WriteLine("| {0} | *{1}*<br>{2} |",
                        paramName,
                        context[paramName],
                        Regex.Replace(root.Value, "\\s+", " ", RegexOptions.Multiline));
                }
                else
                {
                    sw.WriteLine("| {0} | *Unknown type*<br>{1} |",
                        paramName,
                        Regex.Replace(root.Value, "\\s+", " ", RegexOptions.Multiline));
                }

            }
            else if (root.Name == "returns")
            {
                sw.WriteLine("\n> #### Returns");
                sw.WriteLine("> {0}\n", Regex.Replace(root.Value, "\\s+", " ", RegexOptions.Multiline));
            }
            else if (root.Name == "remarks")
            {
                sw.WriteLine("\n> #### Remarks");
                sw.WriteLine("> {0}\n", Regex.Replace(root.Value, "\\s+", " ", RegexOptions.Multiline));
            }
            else if (root.Name == "exception")
            {
                string exName = root.Attribute("cref").Value.Substring(2);
                exName = exName.Replace(context["assembly"] + ".", "");
                exName = exName.Replace(context["typeName"] + ".", "");
                sw.WriteLine("*{0}:* {1}\n",
                    exName,
                    Regex.Replace(root.Value, "\\s+", " ", RegexOptions.Multiline));
            }

            context["lastNode"] = root.Name.ToString();
        }

        private string RearrangeParametersInContext(XElement methodMember)
        {
            string methodPrototype = methodMember.Attribute(XName.Get("name")).Value;
            Match match = Regex.Match(methodPrototype, "\\((.*)\\)");
            string parameterString = match.Groups[1].Value.Replace(" ", "");
            string[] parameterTypes = parameterString.Split(',');

            if (parameterTypes.Length == 0)
            {
                // nothing to do...
                return NormalizeMethod(methodPrototype);
            }

            List<XElement> paramElems = new List<XElement>(methodMember.Elements("param"));
            if (parameterTypes.Length != paramElems.Count)
            {
                // the parameter count do not match, we can't do the rearrangement.
                return NormalizeMethod(methodPrototype);
            }

            string newParamString = "";
            for (int i = 0; i < paramElems.Count; i++)
            {
                XElement paramElem = paramElems[i];
                string paramName = paramElem.Attribute(XName.Get("name")).Value;
                string paramType = parameterTypes[i];
                if (newParamString != "")
                {
                    newParamString += ", ";
                }
                newParamString += paramName;
                context[paramName] = paramType;
            }

            string newMethodPrototype = Regex.Replace(methodPrototype,
                "\\(.*\\)",
                "(" + newParamString + ")");
            return NormalizeMethod(newMethodPrototype);
        }
        private string NormalizeMethod(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new ArgumentNullException();
                }
                return str.Replace("M:", "Method: ");
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
        private string NormalizeCrefMethod(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new ArgumentNullException();
                }
                str = str.Substring(str.IndexOf(':') + 1);
                str = str.Substring(0, (str.Length - (str.Length - str.IndexOf('"'))));
                return string.Format(
                    "[{0}]({1})",
                    Path.GetFileNameWithoutExtension(str),
                    str
                );
            }
            catch(Exception)
            {
                return String.Empty;
            }
        }

    }
}
