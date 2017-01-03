using System.Collections.Generic;

namespace stClient.SpellChecker.SuggestAlgorithms
{
	internal class RemoveExtraChar : SuggestAlgorithm
	{
		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="spellChecker"></param>
        public RemoveExtraChar(global::stClient.SpellChecker.SpellChecker spellChecker)
			: base (spellChecker)
		{
		}


		/// <summary>
		/// Tries to omit one by one char.
		/// </summary>
		/// <param name="word">source word</param>
		/// <param name="suggestedWords">generated suggested words will be placed here</param>
		/// <param name="dic"></param>
		public override void Apply (string word, Dictionary<string, SpellSuggestion> suggestedWords, SpellDictionary dic)
		{
			int wordLen = word.Length;
			if (wordLen <= 1)
				return;

			for (int i = 0; i < wordLen; ++i)
			{
				string newWord = word.Remove (i, 1);

				if (dic.IsWordCorrect (newWord))
				{
					if (!suggestedWords.ContainsKey (newWord))
						suggestedWords.Add (newWord, new SpellSuggestion (newWord, spellChecker.EditDistanceWeights.DeleteCharWeight));
				}
			}

		}
	}
}
