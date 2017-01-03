using System;
using System.Collections.Generic;

namespace stClient.SpellChecker.SuggestAlgorithms
{
	internal class ReplacePatterns : SuggestAlgorithm
	{

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="spellChecker"></param>
        public ReplacePatterns(global::stClient.SpellChecker.SpellChecker spellChecker)
			: base (spellChecker)
		{
		}

		/// <summary>
		/// Applies replace patterns of the passed dictionary.
		/// </summary>
		/// <param name="word">source word</param>
		/// <param name="suggestedWords">generated suggested words will be placed here</param>
		/// <param name="dic"></param>
		public override void Apply (string word, Dictionary<string, SpellSuggestion> suggestedWords, SpellDictionary dic)
		{
			var replacePatterns = dic.ReplacePatterns;
			int n = replacePatterns.Count;
			int p;
			for (int i = 0; i < n; ++i)
			{
				var replacePattern = replacePatterns[i];
				string searchChars = replacePattern.SearchChars;
				string replaceChars = replacePattern.ReplaceChars;
				int editDistance = 0;

				string newWord = word;
				p = 0;
				while ((p = newWord.IndexOf (searchChars, p)) != -1)
				{
					newWord = newWord.Substring (0, p) + replaceChars + newWord.Substring (p + searchChars.Length);
					p += replaceChars.Length;
					editDistance += spellChecker.EditDistanceWeights.ReplaceCharWeight * Math.Max (searchChars.Length, replaceChars.Length);

					if (dic.IsWordCorrect (newWord))
					{
						if (!suggestedWords.ContainsKey (newWord))
							suggestedWords.Add (newWord, new SpellSuggestion (newWord, editDistance));
					}
				}
			}
		}
	}
}
