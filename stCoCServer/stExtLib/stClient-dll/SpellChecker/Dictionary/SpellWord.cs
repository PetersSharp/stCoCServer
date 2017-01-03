using stClient.SpellChecker.Affixes;

namespace stClient.SpellChecker
{
	internal class SpellWord
	{
		public AffixRule[] AffixRules;


		/// <summary>
		/// .ctor
		/// </summary>
		public SpellWord ()
		{
		}




		/// <summary>
		/// Imforms whether the passed rule can be applied to the SpellWord.
		/// </summary>
		/// <param name="rules"></param>
		/// <returns></returns>
		public bool CanApplyRules (params AffixRule[] rules)
		{
			int n = rules.Length;
			for (int i = 0; i < n; ++i)
			{
				var rule = rules[i];
				if (rule!= null && !CanApplyRule (rule))
					return false;
			}
			return true;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public bool CanApplyRule (AffixRule rule)
		{
			int n = AffixRules.Length;
			char checkingRuleName= rule.Name;
			for (int i = 0; i < n; ++i)
			{
				if (AffixRules[i].Name == checkingRuleName)
					return true;
			}

			return false;
		}

	}
}
