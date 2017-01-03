using System.Collections.Generic;

namespace stClient.SpellChecker.SuggestAlgorithms
{
	internal class InsertForgottenChar : SuggestAlgorithm
	{
		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="spellChecker"></param>
        public InsertForgottenChar(global::stClient.SpellChecker.SpellChecker spellChecker)
			: base (spellChecker)
		{
		}


		/// <summary>
		/// Try to insert a character from TryCharacters into an every position.
		/// </summary>
		/// <param name="word">source word</param>
		/// <param name="suggestedWords">generated suggested words will be placed here</param>
		/// <param name="dic"></param>
		public override void Apply (string word, Dictionary<string, SpellSuggestion> suggestedWords, SpellDictionary dic)
		{
			int wordLen = word.Length;
			string tryChars = dic.TryCharacters;
			int tryCharsNum = tryChars.Length;

			for (int i = 0; i <= wordLen; ++i)
			{
				for (int k = 0; k < tryCharsNum; ++k)
				{
					string newWord = word.Insert (i, tryChars[k].ToString());

					if (dic.IsWordCorrect (newWord))
					{
						if (!suggestedWords.ContainsKey (newWord))
							suggestedWords.Add (newWord, new SpellSuggestion (newWord, spellChecker.EditDistanceWeights.InsertCharWeight));
					}
				}
			}
		}
	}
}
