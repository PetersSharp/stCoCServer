using System;
using System.Collections.Generic;

namespace stClient.SpellChecker
{
	public static class TextTools
	{
		private static char[] wordSplitChars = new char[] { ' ', '\r', '\n', ',', '.', '?', '!', '@', ':', '(', ')', '+', '"', '<', '>', '/', '\\', '&', '=' };

		/// <summary>
		/// static .ctor
		/// </summary>
		static TextTools ()
		{
			Array.Sort (wordSplitChars);
		}



		/// <summary>
		/// 
		/// </summary>
		public static char[] WordSplitChars
		{
			get
			{
				return wordSplitChars;
			}
			set
			{
				wordSplitChars= value;
				Array.Sort (wordSplitChars);
			}
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static List<Word> GetWords (string text)
		{
			List<Word> res = new List<Word> ();
			int n = text.Length;
			int p1 = 0;
			for (int i = 0; i < n; ++i)
			{
				char c = text[i];
				if (Array.BinarySearch<char> (wordSplitChars, c) >= 0)
				{
					if (p1 < i)
						res.Add (new Word (p1, i - 1, text));
					p1 = i + 1;
				}
			}
			if (p1 < n)
				res.Add (new Word (p1, n - 1, text));

			return res;
		}




		/// <summary>
		/// Informs whether the passed word is fulle in uppercase.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsUppercaseText (string text)
		{
			int n = text.Length;
			for (int i = 0; i < n; ++i)
			{
				if (Char.IsLower (text[i]))
					return false;
			}

			return true;
		}




		/// <summary>
		/// Informs whether the word is ended with the passed suffix.
		/// The function is equivalent to String.EndsWith but it's much faster.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static bool EndsWith (string text, string suffix)
		{
			int textLen = text.Length;
			int sufLen = suffix.Length;
			if (sufLen > textLen)
				return false;

			for (int i = 1; i <= sufLen; ++i)
			{
				if (suffix[sufLen - i] != text[textLen - i])
					return false;
			}

			return true;
		}




		/// <summary>
		/// Now supports only en-US and ru-RU.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string GetTextCulture (string text)
		{
			int n = text.Length;
			for (int i = 0; i < n; ++i)
			{
				char ch = text[i];
				if ((ch >= 'А' && ch <= 'я') || ch == 'ё' || ch == 'Ё')
					return "ru-RU";
			}

			return "en-US";
		}


	}
}
