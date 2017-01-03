<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:html="http://www.w3.org/1999/xhtml"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="urn:my-scripts"
                exclude-result-prefixes="msxsl user">

  <xsl:output method="text" omit-xml-declaration="yes" indent="yes" encoding="utf-8"/>

  <!-- Script to check string in values -->
  <xsl:include href="DokuWikiSyntaxUtil.xsl"/>

   <xsl:strip-space elements="*" />
   
   <xsl:variable name="newline">
<xsl:text>
</xsl:text>
   </xsl:variable>
   
   <xsl:variable name="tab">
      <xsl:text>	</xsl:text>
   </xsl:variable>
   
   <xsl:template match="/">
      <xsl:apply-templates select="html:html/html:body/node()">
         <xsl:with-param name="context" select="markdown"/>
      </xsl:apply-templates>
   </xsl:template>

   <xsl:template match="@*">
      <xsl:text> </xsl:text>
      <xsl:value-of select="local-name()"/>
      <xsl:text>="</xsl:text>
      <xsl:value-of select="."/>
      <xsl:text>"</xsl:text>
   </xsl:template>
   
   <xsl:template match="text()">
      <!-- it might be possible to use replace-substring to backslash special characters, but that's not implemented yet -->
      <xsl:if test="normalize-space(.)">
         <xsl:value-of select="."/>
      </xsl:if>  
   </xsl:template>
   
   <xsl:template match="text()[parent::html:code[parent::html:pre]]">
      <xsl:call-template name="replace-substring">
         <xsl:with-param name="original">
            <xsl:value-of select="."/>
         </xsl:with-param>
         <xsl:with-param name="substring">
            <xsl:value-of select="$newline"/>
         </xsl:with-param>
         <xsl:with-param name="replacement">
            <xsl:value-of select="$newline"/>
            <xsl:value-of select="$tab"/>
         </xsl:with-param>
      </xsl:call-template>
   </xsl:template>
   
   <xsl:template name="newblock">
      <xsl:param name="context"/>
      <xsl:if test="not(not(preceding-sibling::*) and (parent::html:body or parent::html:li))">
         <xsl:value-of select="$newline"/>
         <xsl:if test="not(self::html:li) or (self::html:li and html:p)">
             <xsl:value-of select="$newline"/>
         </xsl:if>
         <xsl:if test="parent::html:blockquote[parent::html:li and preceding-sibling::*] or (parent::html:li and preceding-sibling::*)">
            <xsl:text>    </xsl:text>
         </xsl:if>
         <xsl:if test="not($context = 'html') and parent::html:blockquote">
            <xsl:text>&gt; </xsl:text>
         </xsl:if>
      </xsl:if>
   </xsl:template>
   
   <!-- if an element isn't templated elsewhere, we move into html context and stay there for any descendent nodes -->
   <xsl:template match="html:*">
      <xsl:if test="self::html:h1 or self::html:h2 or self::html:h3 or self::html:h4 or self::html:h5 or self::html:h6 or self::html:p or self::html:pre or self::html:table or self::html:form or self::html:ul or self::html:ol or self::html:address or self::html:blockquote or self::html:dl or self::html:fieldset or self::html:hr or self::html:noscript">
         <xsl:call-template name="newblock">
            <xsl:with-param name="context" select="'html'"/>
         </xsl:call-template>
      </xsl:if>
      <xsl:call-template name="element"/>
   </xsl:template>
   
   <xsl:template name="element">
      <xsl:text>&lt;</xsl:text>
      <xsl:value-of select="local-name()"/>
      <xsl:apply-templates select="@*"/>
      <xsl:text>&gt;</xsl:text>
      <xsl:apply-templates select="node()">
         <xsl:with-param name="context" select="'html'"/>
      </xsl:apply-templates>
      <xsl:text>&lt;/</xsl:text>
      <xsl:value-of select="local-name()"/>
      <xsl:text>&gt;</xsl:text>
   </xsl:template>
   
   <xsl:template match="html:div">
      <xsl:apply-templates select="node()"/>
   </xsl:template>
   
   <xsl:template match="html:p">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:apply-templates select="node()"/>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>

  <xsl:template match="html:h1">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>= </xsl:text>
            <xsl:apply-templates select="node()"/>
           <xsl:text> =</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:h2">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>== </xsl:text>
            <xsl:apply-templates select="node()"/>
           <xsl:text> ==</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:h3">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>=== </xsl:text>
            <xsl:apply-templates select="node()"/>
           <xsl:text> ===</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:h4">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>==== </xsl:text>
            <xsl:apply-templates select="node()"/>
           <xsl:text> ====</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:h5">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>===== </xsl:text>
            <xsl:apply-templates select="node()"/>
           <xsl:text> =====</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:h6">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>====== </xsl:text>
            <xsl:apply-templates select="node()"/>
           <xsl:text> ======</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:br">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:text> \\</xsl:text>
            <xsl:value-of select="$newline"/>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:blockquote">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
           <xsl:text>&gt;</xsl:text>
           <xsl:apply-templates select="node()"/>
           <xsl:if test="node()[1] = text()">
              <xsl:value-of select="$newline"/>
           </xsl:if>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>

   <xsl:template match="html:table">
    <xsl:param name="context"/>
    <xsl:choose>
      <xsl:when test="$context = 'html'">
        <xsl:call-template name="element"/>
      </xsl:when>
      <xsl:otherwise>

        <xsl:call-template name="newblock"/>

        <xsl:choose>
          <xsl:when test="html:thead/node() != ''">

            <xsl:call-template name="table-tbody">
              <xsl:with-param name="context" select="html:thead/node()"/>
              <xsl:with-param name="separator" select="'^'"/>
            </xsl:call-template>

          </xsl:when>
        </xsl:choose>
        
        <xsl:choose>
          <xsl:when test="html:tbody/node() != ''">

            <xsl:call-template name="table-tbody">
              <xsl:with-param name="context" select="html:tbody/node()"/>
              <xsl:with-param name="separator" select="'|'"/>
            </xsl:call-template>
            
          </xsl:when>
          <xsl:otherwise>

            <xsl:choose>

              <xsl:when test="node() != ''">
                <xsl:call-template name="table-tbody">
                  <xsl:with-param name="context" select="node()"/>
                  <xsl:with-param name="separator" select="'|'"/>
                </xsl:call-template>
              </xsl:when>

            </xsl:choose>

          </xsl:otherwise>
        </xsl:choose>

      </xsl:otherwise>
    </xsl:choose>
   </xsl:template>

  <xsl:template name="table-tbody">
    <xsl:param name="context"/>
    <xsl:param name="separator"/>

    <xsl:choose>
      <xsl:when test="$context != ''">

        <xsl:for-each select="$context">
          <xsl:for-each select="node()">
            <xsl:value-of select="$separator"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="text()"/>
          </xsl:for-each>
          <xsl:text> </xsl:text>
          <xsl:value-of select="$separator"/>
          <xsl:value-of select="$newline"/>
        </xsl:for-each>

      </xsl:when>
      </xsl:choose>

  </xsl:template>

  <!-- this transformation won't backslash the period of a genuine textual newline-number-period combo -->
   
   <xsl:template match="html:ul | html:ol">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:apply-templates select="node()"/>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:li">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:when test="parent::html:ul">
            <xsl:call-template name="newblock"/>
            <xsl:text>* </xsl:text>
            <xsl:apply-templates select="node()"/>
         </xsl:when>
         <xsl:when test="parent::html:ol">
            <xsl:call-template name="newblock"/>
	    	<xsl:value-of select="position()"/>
            <xsl:text>. </xsl:text>
            <xsl:apply-templates select="node()"/>
         </xsl:when>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:pre">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>''</xsl:text>
            <xsl:value-of select="$tab"/>
            <xsl:apply-templates select="node()"/>
           <xsl:text>''</xsl:text>
           <xsl:value-of select="$newline"/>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>

   <xsl:template match="html:code">
    <xsl:param name="context"/>
    <xsl:choose>
      <xsl:when test="$context = 'html'">
        <xsl:call-template name="element"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="newblock"/>
        <xsl:value-of select="$tab"/>
        <xsl:apply-templates select="html:code/node()"/>
        <xsl:value-of select="$newline"/>
      </xsl:otherwise>
    </xsl:choose>
   </xsl:template>

  <xsl:template match="html:hr">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="newblock"/>
            <xsl:text>- - - -</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:a[@href]">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
           <xsl:text>[[</xsl:text>
           <xsl:value-of select="user:ParseUrl(@href)"/>
           <xsl:text>|</xsl:text>
           <xsl:apply-templates select="text()"/>
           <xsl:if test="@title">
             <xsl:text> (</xsl:text>
             <xsl:value-of select="@title"/>
             <xsl:text>)</xsl:text>
           </xsl:if>
           <xsl:text>]]</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <!-- this transformation won't backslash literal asterisks or underscores in text -->

  <xsl:template match="html:u">
    <xsl:param name="context"/>
    <xsl:choose>
      <xsl:when test="$context = 'html'">
        <xsl:call-template name="element"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>__</xsl:text>
        <xsl:apply-templates select="text()"/>
        <xsl:text>__</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="html:i">
    <xsl:param name="context"/>
    <xsl:choose>
      <xsl:when test="$context = 'html'">
        <xsl:call-template name="element"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>//</xsl:text>
        <xsl:apply-templates select="text()"/>
        <xsl:text>//</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="html:strong | html:b | html:em">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:text>**</xsl:text>
            <xsl:apply-templates select="text()"/>
            <xsl:text>**</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>

  <!-- this transformation won't backslash literal backticks in text -->
   
   <xsl:template match="html:code">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
            <xsl:text>''</xsl:text>
            <xsl:apply-templates select="text()"/>
            <xsl:text>''</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
   <xsl:template match="html:img">
      <xsl:param name="context"/>
      <xsl:choose>
         <xsl:when test="$context = 'html'">
            <xsl:call-template name="element"/>
         </xsl:when>
         <xsl:otherwise>
           <xsl:text>{{</xsl:text>
           <xsl:if test="@align and @align != 'left'">
             <xsl:text> </xsl:text>
           </xsl:if>
           <xsl:value-of select="user:ParseUrl(@src)"/>
           <xsl:choose>
             <xsl:when test="@width">
               <xsl:text>?</xsl:text>
               <xsl:value-of select="@width"/>
             </xsl:when>
           </xsl:choose>
           <xsl:choose>
             <xsl:when test="@height">
               <xsl:text>x</xsl:text>
               <xsl:value-of select="@height"/>
             </xsl:when>
           </xsl:choose>
           <xsl:if test="@align and @align != 'right'">
             <xsl:text> </xsl:text>
           </xsl:if>
           <xsl:choose>
             <xsl:when test="@title">
               <xsl:text>|</xsl:text>
               <xsl:value-of select="@title"/>
             </xsl:when>
             <xsl:when test="@alt">
               <xsl:text>|</xsl:text>
               <xsl:value-of select="@alt"/>
             </xsl:when>
           </xsl:choose>
           <xsl:text>}}</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:template>
   
	<!-- The following template is taken from the book "XSLT" by Doug Tidwell (O'Reilly and Associates, August 2001, ISBN 0-596-00053-7) -->
  
	<xsl:template name="replace-substring">
		<xsl:param name="original" />
		<xsl:param name="substring" />
		<xsl:param name="replacement" select="''"/>
		<xsl:variable name="first">
			<xsl:choose>
				<xsl:when test="contains($original, $substring)" >
					<xsl:value-of select="substring-before($original, $substring)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$original"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="middle">
			<xsl:choose>
				<xsl:when test="contains($original, $substring)" >
					<xsl:value-of select="$replacement"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text></xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="last">
			<xsl:choose>
				<xsl:when test="contains($original, $substring)">
					<xsl:choose>
						<xsl:when test="contains(substring-after($original, $substring), $substring)">
							<xsl:call-template name="replace-substring">
								<xsl:with-param name="original">
									<xsl:value-of select="substring-after($original, $substring)" />
								</xsl:with-param>
								<xsl:with-param name="substring">
									<xsl:value-of select="$substring" />
								</xsl:with-param>
								<xsl:with-param name="replacement">
									<xsl:value-of select="$replacement" />
								</xsl:with-param>
							</xsl:call-template>
						</xsl:when>	
						<xsl:otherwise>
							<xsl:value-of select="substring-after($original, $substring)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text></xsl:text>
				</xsl:otherwise>		
			</xsl:choose>				
		</xsl:variable>		
		<xsl:value-of select="concat($first, $middle, $last)"/>
	</xsl:template>

</xsl:stylesheet>
