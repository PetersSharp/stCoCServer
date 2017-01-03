<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="urn:my-scripts"
                exclude-result-prefixes="msxsl user">

  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" encoding="utf-8"/>

  <!-- Script to check string in values -->
  <xsl:include href="DokuWikiSyntaxUtil.xsl"/>

  <xsl:template match="text" name="block">
    <xsl:param name="pString" select="."/>
    <xsl:if test="$pString != ''">
      <xsl:choose>
        <xsl:when test="starts-with($pString,'=')">
          <xsl:call-template name="header">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'>')">
          <xsl:call-template name="quoted">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'- - - -&#xA;')">
          <xsl:call-template name="hrline">
            <xsl:with-param name="pString" select="substring($pString,8)"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test='starts-with($pString,"&#39;&#39;&#xA;")'>
          <xsl:call-template name="code">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString, '&#x3C;html&#x3E;')">
          <xsl:call-template name="nohtml">
            <xsl:with-param name="pString" select="substring($pString,7)"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'&#xA;')">
          <xsl:call-template name="list">
            <xsl:with-param name="pString" select="substring($pString,2)"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="paragraph">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="header">
    <xsl:param name="pString"/>
    <xsl:variable name="vString" select="substring-before($pString,'&#xA;')"/>
    <xsl:variable name="vPatern" select="user:ParseHeadLine($vString, '=')" />
    <xsl:choose>
      <xsl:when test="$vPatern != ''">
        <xsl:variable name="vInside" select="user:ParseHeadPatern($vString, $vPatern)"/>
        <xsl:choose>
          <xsl:when test="starts-with($vPatern,'=====')">
            <h5>
              <xsl:call-template name="inline">
                <xsl:with-param name="pString" select="$vInside"/>
              </xsl:call-template>
            </h5>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'====')">
            <h4>
              <xsl:call-template name="inline">
                <xsl:with-param name="pString" select="$vInside"/>
              </xsl:call-template>
            </h4>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'===')">
            <h3>
              <xsl:call-template name="inline">
                <xsl:with-param name="pString" select="$vInside"/>
              </xsl:call-template>
            </h3>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'==')">
            <h2>
              <xsl:call-template name="inline">
                <xsl:with-param name="pString" select="$vInside"/>
              </xsl:call-template>
            </h2>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'=')">
            <h1>
              <xsl:call-template name="inline">
                <xsl:with-param name="pString" select="$vInside"/>
              </xsl:call-template>
            </h1>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="paragraph">
              <xsl:with-param name="pString" select="$pString"/>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:call-template name="block">
          <xsl:with-param name="pString" select="substring-after($pString,'&#xA;')"/>
        </xsl:call-template>
      </xsl:when>
      
      <xsl:otherwise>
        <xsl:call-template name="paragraph">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
      
    </xsl:choose>
  </xsl:template>

  <xsl:template name="quoted">
    <xsl:param name="pString"/>
    <xsl:variable name="vString" select="substring-before($pString,'&#xA;')"/>
    <xsl:variable name="vPatern" select="user:ParseHeadLine($vString, '>')" />
    <xsl:choose>
      <xsl:when test="$vPatern != ''">
        <xsl:variable name="vInside" select="user:ParseHeadPatern($vString, $vPatern)"/>
        <xsl:choose>
          <xsl:when test="starts-with($vPatern,'>>>>>')">
            <blockquote>
              <blockquote>
                <blockquote>
                  <blockquote>
                    <blockquote>
                      <xsl:call-template name="inline">
                        <xsl:with-param name="pString" select="$vInside"/>
                      </xsl:call-template>
                    </blockquote>
                  </blockquote>
                </blockquote>
              </blockquote>
            </blockquote>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'>>>>')">
            <blockquote>
              <blockquote>
                <blockquote>
                  <blockquote>
                    <xsl:call-template name="inline">
                      <xsl:with-param name="pString" select="$vInside"/>
                    </xsl:call-template>
                  </blockquote>
                </blockquote>
              </blockquote>
            </blockquote>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'>>>')">
            <blockquote>
              <blockquote>
                <blockquote>
                  <xsl:call-template name="inline">
                    <xsl:with-param name="pString" select="$vInside"/>
                  </xsl:call-template>
                </blockquote>
              </blockquote>
            </blockquote>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'>>')">
            <blockquote>
              <blockquote>
                <xsl:call-template name="inline">
                  <xsl:with-param name="pString" select="$vInside"/>
                </xsl:call-template>
              </blockquote>
            </blockquote>
          </xsl:when>
          <xsl:when test="starts-with($vPatern,'>')">
            <blockquote>
              <xsl:call-template name="inline">
                <xsl:with-param name="pString" select="$vInside"/>
              </xsl:call-template>
            </blockquote>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="paragraph">
              <xsl:with-param name="pString" select="$pString"/>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:call-template name="block">
          <xsl:with-param name="pString" select="substring-after($pString,'&#xA;')"/>
        </xsl:call-template>
      </xsl:when>

      <xsl:otherwise>
        <xsl:call-template name="paragraph">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>

    </xsl:choose>
  </xsl:template>

  <xsl:template name="list">
    <xsl:param name="pString"/>
    <xsl:variable name="uCheckList" select="starts-with($pString,'* ')"/>
    <xsl:variable name="nCheckList" select="starts-with($pString,'- ')"/>
    <xsl:choose>
      <xsl:when test="$uCheckList">
        <ul>
          <xsl:call-template name="listItem">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </ul>
        <xsl:call-template name="block">
          <xsl:with-param name="pString">
            <xsl:call-template name="afterlist">
              <xsl:with-param name="pString" select="$pString"/>
            </xsl:call-template>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$nCheckList">
        <ol>
          <xsl:call-template name="listItem">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </ol>
        <xsl:call-template name="block">
          <xsl:with-param name="pString">
            <xsl:call-template name="afterlist">
              <xsl:with-param name="pString" select="$pString"/>
            </xsl:call-template>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="block">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>
  <xsl:template name="paragraph">
    <xsl:param name="pString"/>
    <xsl:choose>
      <xsl:when test="contains($pString,'&#xA;')">
        <p>
          <xsl:call-template name="inline">
            <xsl:with-param name="pString" select="substring-before($pString,'&#xA;')"/>
          </xsl:call-template>
        </p>
      </xsl:when>
      <xsl:otherwise>
        <p>
          <xsl:call-template name="inline">
            <xsl:with-param name="pString" select="$pString"/>
          </xsl:call-template>
        </p>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:call-template name="block">
      <xsl:with-param name="pString" select="substring-after($pString,'&#xA;')"/>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="afterlist">
    <xsl:param name="pString"/>
    <xsl:choose>
      <xsl:when test="starts-with($pString,'- ') or starts-with($pString,'* ')">
        <xsl:call-template name="afterlist">
          <xsl:with-param name="pString" select="substring-after($pString,'&#xA;')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$pString"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="listItem">
    <xsl:param name="pString"/>
    <xsl:if test="starts-with($pString,'- ') or starts-with($pString,'* ')">
      <li>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString"
          select="substring-before(substring($pString,3),'&#xA;')"/>
        </xsl:call-template>
      </li>
      <xsl:call-template name="listItem">
        <xsl:with-param name="pString"
        select="substring-after($pString,'&#xA;')"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="inline">
    <xsl:param name="pString" select="."/>
    <xsl:if test="$pString != ''">
      <xsl:variable name="vElement" select="substring($pString,3)"/>

      <xsl:choose>
        <xsl:when test="starts-with($pString,'**')">
          <xsl:call-template name="strong">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'//')">
          <xsl:call-template name="italic">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'__')">
          <xsl:call-template name="underline">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'[[')">
          <xsl:call-template name="link">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'{{')">
          <xsl:call-template name="image">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'\\')">
          <xsl:call-template name="linebreak">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test="starts-with($pString,'%%')">
          <xsl:call-template name="nowiki">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:when test='starts-with($pString,"&#39;&#39;")'>
          <xsl:call-template name="code">
            <xsl:with-param name="pString" select="$vElement"/>
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($pString,1,1)"/>
          <xsl:call-template name="inline">
            <xsl:with-param name="pString" select="substring($pString,2)"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="nohtml">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,'&#x3C;/html&#x3E;')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <xsl:value-of select="$vInside"/>
        <xsl:call-template name="block">
          <xsl:with-param name="pString" select="substring-after($pString,'&#x3C;/html&#x3E;')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'&#x3C;html&#x3E;'"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="nowiki">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,'%%')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <xsl:value-of select="$vInside"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="substring-after($pString,'%%')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'%%'"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="strong">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,'**')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <strong>
          <xsl:value-of select="$vInside"/>
        </strong>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString"
          select="substring-after($pString,'**')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'**'"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="italic">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,'//')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <i>
          <xsl:value-of select="$vInside"/>
        </i>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="substring-after($pString,'//')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'//'"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="underline">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,'__')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <u>
          <xsl:value-of select="$vInside"/>
        </u>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="substring-after($pString,'__')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'__'"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="hrline">
    <xsl:param name="pString"/>
    <hr/>
    <xsl:call-template name="block">
      <xsl:with-param name="pString" select="$pString"/>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="linebreak">
    <xsl:param name="pString"/>
    <br/>
    <xsl:call-template name="inline">
      <xsl:with-param name="pString" select="$pString"/>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="code">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select='substring-before($pString,"&#39;&#39;")'/>

    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <pre>
          <xsl:value-of select="$vInside"/>
        </pre>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select='substring-after($pString,"&#39;&#39;")'/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="&#39;&#39;"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>
  <xsl:template name="link">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,']]')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <xsl:call-template name="href">
          <xsl:with-param name="pString" select="substring-after($pString,']]')"/>
          <xsl:with-param name="pInside" select="$vInside"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'[['"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="href">
    <xsl:param name="pString"/>
    <xsl:param name="pInside"/>
    <xsl:variable name="vHref" select="substring-before($pInside,'|')"/>
    <xsl:variable name="vDesc" select="substring-after($pInside,'|')"/>
    <xsl:choose>
      <xsl:when test="$vHref != ''">
        <xsl:variable name="oHref" select="user:ParseUrl($vHref)"/>
        <a href="{$oHref}">
          <xsl:choose>
            <xsl:when test="$vDesc != ''">
              <xsl:value-of select="$vDesc"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$vHref"/>
            </xsl:otherwise>
          </xsl:choose>
        </a>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$pInside != ''">
        <a href="{$pInside}">
          <xsl:value-of select="$pInside"/>
        </a>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('&quot;',$pString,'&quot;')"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template name="image">
    <xsl:param name="pString"/>
    <xsl:variable name="vInside" select="substring-before($pString,'}}')"/>
    <xsl:choose>
      <xsl:when test="$vInside != ''">
        <xsl:call-template name="imageparse">
          <xsl:with-param name="pString" select="substring-after($pString,'}}')"/>
          <xsl:with-param name="pInside" select="$vInside"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="'{{'"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="imageparse">
    <xsl:param name="pString"/>
    <xsl:param name="pInside"/>

    <xsl:variable name="vUri" select="substring-before($pInside,'|')"/>
    <xsl:variable name="oDesc" select="substring-after($pInside,'|')"/>

    <xsl:variable name="tUri">
      <xsl:choose>
        <xsl:when test="$vUri != ''">
          <xsl:value-of select="$vUri"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$pInside"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="$tUri != ''">

        <xsl:variable name="oAlign">
          <xsl:value-of select="user:ParseImageAlign($tUri)" />
        </xsl:variable>

        <xsl:variable name="oUri">
          <xsl:value-of select="normalize-space($tUri)" />
        </xsl:variable>

        <xsl:variable name="vUrl" select="substring-before($oUri,'?')"/>
        <xsl:variable name="oUrl">
          <xsl:choose>
            <xsl:when test="$vUrl != ''">
              <xsl:value-of select="user:ParseUrl($vUrl)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="user:ParseUrl($oUri)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="vArgs" select="substring-after($oUri,'?')"/>
        <xsl:variable name="oWidth">
          <xsl:choose>
            <xsl:when test="$vArgs != ''">
              <xsl:value-of select="user:ParseImageSize($vArgs, true)" />
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="oHeight">
          <xsl:choose>
            <xsl:when test="$vArgs != ''">
              <xsl:value-of select="user:ParseImageSize($vArgs, false)" />
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="$oWidth != '' and $oHeight != '' and $oAlign != ''">
            <img src="{$oUrl}" alt="{$oDesc}" width="{$oWidth}" height="{$oHeight}" align="{$oAlign}" />
          </xsl:when>
          <xsl:when test="$oWidth != '' and $oHeight != ''">
            <img src="{$oUrl}" alt="{$oDesc}" width="{$oWidth}" height="{$oHeight}" />
          </xsl:when>
          <xsl:when test="$oWidth != '' and $oAlign != ''">
            <img src="{$oUrl}" alt="{$oDesc}" width="{$oWidth}" />
          </xsl:when>
          <xsl:when test="$oWidth != ''">
            <img src="{$oUrl}" alt="{$oDesc}" width="{$oWidth}" />
          </xsl:when>
          <xsl:when test="$oDesc != '' and $oAlign != ''">
            <img src="{$oUrl}" alt="{$oDesc}" />
          </xsl:when>
          <xsl:when test="$oDesc != ''">
            <img src="{$oUrl}" alt="{$oDesc}" />
          </xsl:when>
          <xsl:when test="$oAlign != ''">
            <img src="{$oUrl}" align="{$oAlign}" />
          </xsl:when>
          <xsl:otherwise>
            <img src="{$oUrl}" />
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test="contains($pString,'&#xA;')">
            <xsl:call-template name="block">
              <xsl:with-param name="pString" select="$pString"/>
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="inline">
              <xsl:with-param name="pString" select="$pString"/>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="concat('&quot;',$pString,'&quot;')"/>
        <xsl:call-template name="inline">
          <xsl:with-param name="pString" select="$pString"/>
        </xsl:call-template>
      </xsl:otherwise>
      
    </xsl:choose>

  </xsl:template>

</xsl:stylesheet>
