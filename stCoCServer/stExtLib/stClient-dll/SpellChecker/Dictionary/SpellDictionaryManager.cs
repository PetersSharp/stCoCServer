using System;
using System.Collections.Generic;
using System.IO;

namespace stClient.SpellChecker
{
	/// <summary>
	/// Singletone
	/// </summary>
	internal sealed class SpellDictionaryManager
	{
		private readonly Dictionary<string, SpellDictionary> dictionaries;


		/// <summary>
		/// .ctor
		/// </summary>
		public SpellDictionaryManager(string dictPath)
		{
			dictionaries = new Dictionary<string, SpellDictionary> ();
            DictionariesBaseFolder = dictPath;
        }

		/// <summary>
		/// Returns dictionary for the passed culture.
		/// </summary>
		/// <param name="cultName">e.g., "en-US", or "ru-RU"</param>
		/// <returns>null, if there's no such dictionary</returns>
		public SpellDictionary GetDictionary (string cultName)
		{
			SpellDictionary dict = null;
			cultName = cultName.ToLower();
			if (dictionaries.ContainsKey(cultName))
			{
				dict = dictionaries[cultName];
			}
			else
			{
				try
				{
					dict = new SpellDictionary(cultName);
					
					string path = Path.Combine(DictionariesBaseFolder, cultName);
                    if (!Directory.Exists(path))
                    {
                        throw new DirectoryNotFoundException();
                    }
					dict.Load(path);
					dictionaries.Add (cultName, dict);
				}
				catch (Exception e)
				{
                    throw e;
				}
			}
			return dict;
		}

		/// <summary>
		/// 
		/// </summary>
		public string DictionariesBaseFolder
		{
			get; set;
		}

		/// <summary>
		/// 
		/// </summary>
		public void ReloadDictionaries()
		{
			dictionaries.Clear();
		}



	}
}
