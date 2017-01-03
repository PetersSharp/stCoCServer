<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="urn:my-scripts"
                exclude-result-prefixes="msxsl user">

  <!-- Script to check string in values -->
  <msxsl:script language="C#" implements-prefix="user">
    <![CDATA[
      public string ParseImageSize(string part, bool iswidth)
      {
          if (string.IsNullOrWhiteSpace(part))
          {
              return "";
          }
          string[] lst = part.Split('x');
          if ((iswidth) && (lst.Length > 0))
          {
            return lst[0];
          }
          else if ((!iswidth) && (lst.Length > 1))
          {
            return lst[1];
          }
          else if (!iswidth)
          {
            return "";
          }
          return part;
      }
      public string ParseImageAlign(string part)
      {
          if (string.IsNullOrWhiteSpace(part))
          {
              return "";
          }
          else if (part.StartsWith(" ") && part.EndsWith(" "))
          {
              return "center";
          }
          else if (part.StartsWith(" "))
          {
              return "left";
          }
          else if (part.EndsWith(" "))
          {
              return "right";
          }
          return "";
      }
      public string ParseHeadLine(string part, string ch)
      {
          if ((string.IsNullOrWhiteSpace(part)) || (string.IsNullOrEmpty(ch)))
          {
              return "";
          }
          for (int i = 0; i < part.Length; i++)
          {
              if (part[i] != ch[0])
              {
                  if (i == 0)
                  {
                      return "";
                  }
                  return part.Substring(0, i);
              }
          }
          return "";
      }
      public string ParseHeadPatern(string part, string pattern)
      {
          return part.Replace(pattern, String.Empty).Trim();
      }
      public static string ParseUrl(string url)
      {
          return Regex.Replace(url.Trim(), @"[^-A-Za-z0-9+&@#/%?=~_|!:,.;\(\)]", "");
      }      
    ]]>
  </msxsl:script>
</xsl:stylesheet>
