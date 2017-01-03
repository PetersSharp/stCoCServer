using System.Collections.Generic;
using System.Text;

namespace stClient.SpellChecker.SuggestAlgorithms
{
	internal class SwapChars : SuggestAlgorithm
	{
		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="spellChecker"></param>
        public SwapChars(global::stClient.SpellChecker.SpellChecker spellChecker)
			: base (spellChecker)
		{
		}


		/// <summary>
		/// Tries to swap every pair of adjacent chars.
		/// </summary>
		/// <param name="word">source word</param>
		/// <param name="suggestedWords">generated suggested words will be placed here</param>
		/// <param name="dic"></param>
		public override void Apply (string word, Dictionary<string, SpellSuggestion> suggestedWords, SpellDictionary dic)
		{
			int wordLen = word.Length;
			for (int i = 0; i < wordLen - 1; ++i)
			{
				// swapping...
				StringBuilder s = new StringBuilder (word);
				char t = s[i];
				s[i] = s[i + 1];
				s[i + 1] = t;
				string newWord = s.ToString ();

				if (dic.IsWordCorrect (newWord))
				{
					if (!suggestedWords.ContainsKey (newWord))
						suggestedWords.Add (newWord, new SpellSuggestion (newWord, spellChecker.EditDistanceWeights.SwapCharWeight));
				}
			}
		}
	}
}
