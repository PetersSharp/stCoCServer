
/* 
* SpellChecker
* ============
* 
* SpellChecker is a lightweight .NET library that checks and corrects spelling errors in any human language (dictionaries for English and Russian languages are provided for now).
* The following functionality is provided:
* - Different word parts are considered (such as suffixes and prefixes) during the spell check.
* - Auto-detection of the checking text language
* - Correction options are suggested for misspelled words for the following cases:
*  - merged words (missed space character)
*  - missed character in a word
*  - replaced character
*  - extra character
*  - swapped neighbor characters
* - No database is required: all dictionaries are stored in plain text files
* - New languages can be easily added just by adding dictionary files
* 
* Origin: https://github.com/mshmelev/SpellChecker
* 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using stClient.SpellChecker.SuggestAlgorithms;
using System.IO;

namespace stClient.SpellChecker
{
	public class SpellChecker
	{
		private readonly EditDistanceWeights editDistanceWeights;
		private readonly SpellOptions spellOptions;
        private readonly List<SuggestAlgorithm> suggestAlgorithms;
		private readonly SpellDictionaryManager dictionaryManager;

		/// <summary>
		/// .ctor
		/// </summary>
        public SpellChecker(string dictPath = null)
		{
            try
            {
                if (string.IsNullOrWhiteSpace(dictPath))
                {
                    dictPath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                    dictPath = Path.Combine(new string[] { Path.GetDirectoryName(dictPath), "data", "spell" });
                }
                if (!Directory.Exists(dictPath))
                {
                    throw new DirectoryNotFoundException();
                }
                editDistanceWeights = new EditDistanceWeights();
                spellOptions = new SpellOptions();

                suggestAlgorithms = new List<SuggestAlgorithm>()
                {
				    new InsertForgottenChar (this),
				    new RemoveExtraChar (this),
				    new ReplacePatterns (this),
				    new ReplaceWrongChars (this),
				    new SplitWords (this),
				    new SwapChars (this)
                };

                StoreUserWordsOnDisk = true;
                dictionaryManager = new SpellDictionaryManager(dictPath);
            }
            catch (Exception e)
            {
                throw e;
            }
		}



		/// <summary>
		/// 
		/// </summary>
		public SpellOptions SpellOptions
		{
			get
			{
				return spellOptions;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public EditDistanceWeights EditDistanceWeights
		{
			get
			{
				return editDistanceWeights;
			}
		}



		/// <summary>
		/// Checks spelling of the passed text.
		/// The culture of each world in the text is detecting automatically.
		/// </summary>
		/// <param name="text"></param>
		/// <returns>List of wrong-spelled words</returns>
		public List<Word> CheckTextSpell (string text)
		{
			List<Word> words= TextTools.GetWords (text);
			List<Word> badWords = new List<Word> ();

			foreach (Word w in words)
			{
				if (!CheckWordSpell (w.Text))
					badWords.Add (w);
			}

			return badWords;
		}



		/// <summary>
		/// Checks spelling of the passed text with the passed culture.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="culture">culture of the whole text</param>
		/// <returns>List of wrong-spelled words</returns>
		public List<Word> CheckTextSpell (string text, string culture)
		{
			List<Word> words = TextTools.GetWords (text);
			List<Word> badWords = new List<Word> ();

			foreach (Word w in words)
			{
				if (!CheckWordSpell (w.Text, culture))
					badWords.Add (w);
			}

			return badWords;
		}


		/// <summary>
		/// Checks the spelling of the passed word. Culture is auto detecting.
		/// </summary>
		/// <param name="word"></param>
		/// <returns>true, if word is correct</returns>
		public bool CheckWordSpell (string word)
		{
			return CheckWordSpell (word, TextTools.GetTextCulture (word));
		}



		/// <summary>
		/// Checks the spelling of the passed word with the passed culture.
		/// </summary>
		/// <param name="word"></param>
		/// <param name="culture"></param>
		/// <returns>true, if word is correct</returns>
		public bool CheckWordSpell (string word, string culture)
		{
			var dic = dictionaryManager.GetDictionary (culture);
			if (dic == null)
				throw new Exception (String.Format (Strings.ErrorDictionaryMissing, culture));

			if (spellOptions.IgnoreUppercaseWords && TextTools.IsUppercaseText (word))
				return true;

			bool isCorrect= dic.IsWordCorrect (word);
			if (!isCorrect && !spellOptions.CaseSensitive)
				isCorrect = dic.IsWordCorrect (word.ToLower());

			return isCorrect;
		}



		/// <summary>
		/// Suggests diferrent spelling forms of the passed wrong wpelled word.
		/// Culture is auto detecting.
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		public SpellSuggestion[] SuggestCorrectedWords (string word)
		{
			return SuggestCorrectedWords(word, false);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="word"></param>
		/// <param name="suggestForCorrectWord"></param>
		/// <returns></returns>
		public SpellSuggestion[] SuggestCorrectedWords (string word, bool suggestForCorrectWord)
		{
			return SuggestCorrectedWords(word, TextTools.GetTextCulture(word), suggestForCorrectWord);
		}



		/// <summary>
		/// Suggests diferrent spelling forms of the passed wrong wpelled word.
		/// </summary>
		/// <param name="word">misspelled word</param>
		/// <param name="culture">culture of the passed word</param>
		/// <returns></returns>
		public SpellSuggestion[] SuggestCorrectedWords (string word, string culture)
		{
			return SuggestCorrectedWords(word, culture, false);
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="word"></param>
		/// <param name="culture"></param>
		/// <param name="suggestForCorrectWord"></param>
		/// <returns></returns>
		public SpellSuggestion[] SuggestCorrectedWords (string word, string culture, bool suggestForCorrectWord)
		{
			// check if word is correct already
			var dic = dictionaryManager.GetDictionary(culture);
			if (dic == null || (!suggestForCorrectWord && CheckWordSpell(word, culture)))
				return new SpellSuggestion[0];

			// get sugestions
			Dictionary<string, SpellSuggestion> suggestedWords = new Dictionary<string, SpellSuggestion>();

			foreach (var alg in suggestAlgorithms)
				alg.Apply(word, suggestedWords, dic);

			var v = suggestedWords.Values.OrderBy(w => w.EditDistance).ThenBy(w => w.Text);
			return v.ToArray();
		}




		/// <summary>
		/// Adds the passed word to user dictionary.
		/// The word's culture is auto detecting.
		/// </summary>
		/// <param name="word"></param>
		/// <returns>false, if word wasn't added (e.g. the word already exists)</returns>
		public bool AddUserWord (string word)
		{
			return AddUserWord (word, TextTools.GetTextCulture (word));
		}



		/// <summary>
		/// Adds the passed words to user dictionary.
		/// The words' culture is auto detecting.
		/// </summary>
		/// <param name="words"></param>
		/// <returns>false, if no word was added (all of the word already exist)</returns>
		public bool AddUserWord(IEnumerable<string> words)
		{
			bool res = false;
			foreach (var word in words)
				res |= AddUserWord (word, TextTools.GetTextCulture (word));
			return res;
		}



		/// <summary>
		/// Adds the passed word to user dictionary.
		/// </summary>
		/// <param name="word"></param>
		/// <param name="culture">Culture of the word.</param>
		/// <returns>false, if word wasn't added (e.g. the word already exists)</returns>
		public bool AddUserWord (string word, string culture)
		{
			var dic = dictionaryManager.GetDictionary (culture);
			if (dic == null)
				throw new Exception (String.Format (Strings.ErrorDictionaryMissing, culture));

			return dic.AddUserWord (word, StoreUserWordsOnDisk);
		}

		/// <summary>
		/// Adds the passed words to user dictionary.
		/// </summary>
		/// <param name="words"></param>
		/// <param name="culture">Culture of all of the words.</param>
		/// <returns>false, if no word was added (all of the word already exist)</returns>
		public bool AddUserWord(IEnumerable<string> words, string culture)
		{
			bool res = false;
			foreach (var word in words)
				res |= AddUserWord(word, culture);
			return res;
		}

		/// <summary>
		/// 
		/// </summary>
		public string DictionariesPath
		{
			get
			{
				return dictionaryManager.DictionariesBaseFolder;
			}
			set
			{
				dictionaryManager.DictionariesBaseFolder= value;
			}
		}

        /// <summary>
		/// 
		/// </summary>
		public bool StoreUserWordsOnDisk
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public void ReloadDictionaries()
		{
			dictionaryManager.ReloadDictionaries();
		}

	}
}
