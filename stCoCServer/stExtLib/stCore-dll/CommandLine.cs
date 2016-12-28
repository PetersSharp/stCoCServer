using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace stCore
{
    // Origin: https://bitbucket.org/lord_vetinari/commandlineparser/overview
    //     or: http://lukasz-lademann.blogspot.ru/2013/01/c-command-line-arguments-parser.html
    // Command Line Arguments Parser
    //
    public static class CommandLine
    {
        public class Switch
        {
            public Switch(string name, Action<IEnumerable<string>> handler, string shortForm)
            {
                Name = name;
                Handler = handler;
                ShortForm = shortForm;
            }
            public Switch(string name, Action<IEnumerable<string>> handler)
            {
                Name = name;
                Handler = handler;
                ShortForm = null;
            }
            public string Name
            {
                get;
                private set;
            }
            public string ShortForm
            {
                get;
                private set;
            }
            public Action<IEnumerable<string>> Handler
            {
                get;
                private set;
            }
            public int InvokeHandler(string[] values)
            {
                Handler(values);
                return 1;
            }
        }
        private static readonly Regex ArgRegex = 
            new Regex(@"(?<name>[^=]+)=?((?<quoted>\""?)(?<value>(?(quoted)[^\""]+|[^,]+))\""?,?)*",
                RegexOptions.Compiled | RegexOptions.CultureInvariant |
                RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private const string NameGroup = "name";
        private const string ValueGroup = "value";

        public static void Process(this string[] args, Action printUsage, params Switch[] switches)
        {
            if ((from arg in args
                 from Match match in ArgRegex.Matches(arg)
                 from s in switches
                 where match.Success &&
                     ((string.Compare(match.Groups[NameGroup].Value, s.Name, true) == 0) ||
                     (string.Compare(match.Groups[NameGroup].Value, s.ShortForm, true) == 0))
                 select s.InvokeHandler(match.Groups[ValueGroup].Value.Split(','))).Sum() == 0)
            {
                if (printUsage != null) { printUsage(); }
            }
        }
    }
}

/* Example usage:
             args.Process(
               () => Console.WriteLine("Usage is switch1=value1,value2 switch2=value3"),
               new CommandLine.Switch("switch1",
                   val => Console.WriteLine("switch 1 with value {0}",
                       string.Join(" ", val))),
               new CommandLine.Switch("switch2",
                   val => Console.WriteLine("switch 2 with value {0}",
                       string.Join(" ", val)), "s1"));

 */
