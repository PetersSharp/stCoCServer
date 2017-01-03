
using System.Collections.Generic;

namespace stClient.SpellChecker.SuggestAlgorithms
{
	internal class SplitWords : SuggestAlgorithm
	{
		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="spellChecker"></param>
        public SplitWords(global::stClient.SpellChecker.SpellChecker spellChecker)
            : base(spellChecker)
		{
		}



		/// <summary>
		/// Try to split the passed word onto 2 words.
		/// </summary>
		/// <param name="word">source word</param>
		/// <param name="suggestedWords">generated suggested words will be placed here</param>
		/// <param name="dic"></param>
		public override void Apply (string word, Dictionary<string, SpellSuggestion> suggestedWords, SpellDictionary dic)
		{
			int wordLen = word.Length;
			if (wordLen < 2)
				return;

			for (int i = 1; i < wordLen; ++i)
			{
				string word1 = word.Substring (0, i);
				string word2 = word.Substring (i);

				if (dic.IsWordCorrect (word1) && dic.IsWordCorrect (word2))
				{
					string newText = word1 + ' ' + word2;
					if (!suggestedWords.ContainsKey (newText))
						suggestedWords.Add (newText, new SpellSuggestion (newText, spellChecker.EditDistanceWeights.InsertCharWeight));
				}
			}
		}

	}
}
