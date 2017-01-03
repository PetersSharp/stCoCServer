using System.Collections.Generic;
using System.Text;

namespace stClient.SpellChecker.SuggestAlgorithms
{
	internal class ReplaceWrongChars : SuggestAlgorithm
	{
		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="spellChecker"></param>
        public ReplaceWrongChars(global::stClient.SpellChecker.SpellChecker spellChecker)
			: base (spellChecker)
		{
		}



		/// <summary>
		/// Tries to replace every char of the passed word to chars from the TryCharacters of the dictionary.
		/// </summary>
		/// <param name="word">source word</param>
		/// <param name="suggestedWords">generated suggested words will be placed here</param>
		/// <param name="dic"></param>
		public override void Apply (string word, Dictionary<string, SpellSuggestion> suggestedWords, SpellDictionary dic)
		{
			int srcWordLen = word.Length;
			string tryChars = dic.TryCharacters;
			int tryCharsNum = tryChars.Length;

			for (int i = 0; i < srcWordLen; ++i)
			{
				StringBuilder s = new StringBuilder (word);
				for (int k = 0; k < tryCharsNum; ++k)
				{
					char ch = tryChars[k];
					if (s[i] == ch)
						continue;

					s[i] = ch;
					string newWord = s.ToString ();
					if (dic.IsWordCorrect (newWord))
					{
						if (!suggestedWords.ContainsKey (newWord))
							suggestedWords.Add (newWord, new SpellSuggestion (newWord, spellChecker.EditDistanceWeights.ReplaceCharWeight));
					}
				}
			}
		}
	}
}
