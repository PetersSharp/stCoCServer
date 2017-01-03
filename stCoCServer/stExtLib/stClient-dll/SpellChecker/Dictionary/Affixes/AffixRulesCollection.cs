using System.Collections.Generic;

namespace stClient.SpellChecker.Affixes
{
	internal class AffixRulesCollection : List<AffixRule>
	{
		/// <summary>
		/// .ctor
		/// </summary>
		public AffixRulesCollection ()
		{
		}


		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="collection"></param>
		public AffixRulesCollection (IEnumerable<AffixRule> collection)
			: base (collection)
		{
		}

	}
}
