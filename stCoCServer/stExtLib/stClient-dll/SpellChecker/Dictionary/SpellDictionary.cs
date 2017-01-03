using System;
using System.Collections.Generic;
using System.Linq;
using stClient.SpellChecker.Affixes;
using System.IO;

namespace stClient.SpellChecker
{
	internal class SpellDictionary
	{
		private AffixRulesCollection affixRules;
		private AffixRule[] affixRulesLookup;
		private Dictionary<string, SpellWord> words;
		private string tryCharacters;
		private List<ReplacePattern> replacePatterns;
		private string cultName;
		private string userDicPath;

		#region ReplacePattern class
		/// <summary>
		/// Replace pattern for spelling suggestions.
		/// </summary>
		internal class ReplacePattern
		{
			public string SearchChars;
			public string ReplaceChars;

			public ReplacePattern (string searchChars, string replaceChars)
			{
				SearchChars = searchChars;
				ReplaceChars = replaceChars;
			}
		}
		#endregion


		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="cultName"></param>
		public SpellDictionary (string cultName)
		{
			this.cultName = cultName;
		}



		/// <summary>
		/// Loads dictinary from the passed folder.
		/// </summary>
		/// <param name="dirPath">Folder where .aff and .dic files are located</param>
		public void Load (string dirPath)
		{
			// load affixes
			string[] ff= Directory.GetFiles (dirPath, "*.aff");
			if (ff.Length == 0)
				throw new FileNotFoundException (Strings.ErrorAffFileMissing);
			LoadAffixesFile (ff[0]);

			// load dictionary
			ff = Directory.GetFiles (dirPath, "*.dic");
			if (ff.Length == 0)
				throw new FileNotFoundException (Strings.ErrorDicFileMissing);
			LoadWordsFile (ff[0]);

			// load user dictionary
			userDicPath = String.Format (@"{0}\{1}.usr", dirPath.TrimEnd ('/', '\\'), cultName);
			if (File.Exists (userDicPath))
				LoadUserWordsFile (userDicPath);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		private void LoadAffixesFile (string path)
		{
			StreamReader f= File.OpenText (path);
			string s;
			char[] sepChars= new char[] {' ', '\t'};
			AffixRule curAffixRule = null;
			affixRules = new AffixRulesCollection ();
			affixRulesLookup = new AffixRule['Z'+1];
			tryCharacters = "";
			replacePatterns = new List<ReplacePattern> ();

			while ((s= f.ReadLine())!= null)
			{
				// ignore comment
				if (s.StartsWith ("#"))
					continue;

				string[] data = s.Split (sepChars, StringSplitOptions.RemoveEmptyEntries);
				if (data.Length == 0)
					continue;			// empty string

				if (data[0] == "PFX" || data[0] == "SFX")
				{
					if (data.Length == 4)
					{
						// add new rule
						curAffixRule = AffixRule.Parse (data);
						affixRules.Add (curAffixRule);
						affixRulesLookup[curAffixRule.Name] = curAffixRule;
					}
					else if (curAffixRule != null)
					{
						// add affix
						curAffixRule.ParseAndAddAffix (data);
					}
				}
				else
				{
					if (data[0] == "TRY")
						tryCharacters = data[1];
					else if (data[0] == "REP" && data.Length == 3)
						replacePatterns.Add (new ReplacePattern (data[1], data[2]));
					
					curAffixRule = null;
				}
			}

			f.Close ();
		}





		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		private void LoadWordsFile (string path)
		{
			StreamReader f = File.OpenText (path);
			words = new Dictionary<string, SpellWord> ();
			string word;

			f.ReadLine ();		// read 1st line with words count
			while ((word = f.ReadLine ()) != null)
			{
				int p = word.IndexOf ('/');
				SpellWord spellWord= new SpellWord();
				AffixRulesCollection wordRules = new AffixRulesCollection();
				if (p != -1)
				{
					int n = word.Length;
					for (int i = p+1; i < n; ++i)
					{
						char c = word[i];
						if (c< affixRulesLookup.Length)
						{
							AffixRule rule= affixRulesLookup[c];
							if (rule!= null)
								wordRules.Add (rule);
						}
					}
					word = word.Substring (0, p);
				}
				spellWord.AffixRules = wordRules.ToArray<AffixRule>();

				try
				{
					words.Add (word, spellWord);
				}
				catch (Exception)
				{
				}
			}

			f.Close ();
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		private void LoadUserWordsFile (string path)
		{
			using (StreamReader f = File.OpenText (path))
			{
				string s;
				while ((s = f.ReadLine ()) != null)
				{
					if (!words.ContainsKey (s))
					{
						SpellWord spellWord = new SpellWord ();
						spellWord.AffixRules = new AffixRule[0];
						words.Add (s, spellWord);
					}
				}
			}
		}



		/// <summary>
		/// Adds new user word to the dictionary.
		/// The word is saving in the .usr file.
		/// </summary>
		/// <param name="word"></param>
		/// <param name="saveOnDisk"></param>
		/// <returns>false, if word wasn't added (e.g. the word already exists)</returns>
		public bool AddUserWord (string word, bool saveOnDisk)
		{
			// checking...
			word = word.Trim ();
			if (word == "")
				return false;
			if (words.ContainsKey (word))
				return false;

			// adding to words list
			SpellWord spellWord = new SpellWord ();
			spellWord.AffixRules = new AffixRule[0];
			words.Add (word, spellWord);

			// save to disk
			if (saveOnDisk)
			{
				using (StreamWriter f = File.AppendText (userDicPath))
					f.WriteLine (word);
			}

			return true;
		}



		/// <summary>
		/// List of characters to try when generating suggestions.
		/// </summary>
		public string TryCharacters
		{
			get
			{
				return tryCharacters;
			}
		}



		/// <summary>
		/// Replace patterns which uses in the spell suggestion.
		/// </summary>
		public List<ReplacePattern> ReplacePatterns
		{
			get
			{
				return replacePatterns;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		public bool IsWordCorrect (string word)
		{
			// check in the main dictionary
			if (CheckWordInternal (word))
				return true;

			// try to uncapitalize the 1st letter
			if (Char.IsUpper (word[0]))
			{
				string lowWord = Char.ToLower (word[0]) + word.Substring (1);
				return CheckWordInternal (lowWord);
			}
			return false;
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		private bool CheckWordInternal (string word)
		{
			if (words.ContainsKey (word))
				return true;

			Dictionary<string, AffixRule> wordForms = new Dictionary<string, AffixRule> ();
			wordForms.Add (word, null);

			while (wordForms.Count > 0)
			{
				string checkingForm = wordForms.Keys.First ();
				AffixRule appliedAffixRule = wordForms[checkingForm];

				for (int i = 0; i < affixRules.Count; ++i)
				{
					var affixRule = affixRules[i];
					if (!CanCombineRules (affixRule, appliedAffixRule))
						continue;

					foreach (Affix affix in affixRule.Affixes)
					{
						string wordBase = affix.ApplyReverse (checkingForm);
						if (wordBase != null)
						{
							// check the word presents in dictionary
							SpellWord spellWord;
							if (words.TryGetValue (wordBase, out spellWord))
							{
								if (spellWord.CanApplyRules (appliedAffixRule, affixRule))
									return true;
							}

							// add new word form
							if (appliedAffixRule == null && !wordForms.ContainsKey (wordBase))
								wordForms.Add (wordBase, affixRule);
						}
					}
				}

				// remove checked word
				wordForms.Remove (checkingForm);
			}

			return false;
		}



		/// <summary>
		/// Informs whether the newRule can be combined with appliedRule (to a word).
		/// </summary>
		/// <param name="newRule"></param>
		/// <param name="appliedRule"></param>
		/// <returns></returns>
		private bool CanCombineRules (AffixRule newRule, AffixRule appliedRule)
		{
			if (newRule == null || appliedRule == null)
				return true;

			// Only 1 rule of each type (SuffixRule of PrefixRule) can be combined.
			if (newRule is SuffixRule && appliedRule is SuffixRule)
				return false;
			if (newRule is PrefixRule && appliedRule is PrefixRule)
				return false;

			// SuffixRule and PrefixRule can be comined if they are both combinable (CanCombine= true).
			if (newRule.CanCombine && appliedRule.CanCombine)
				return true;

			return false;
		}


	}
}
