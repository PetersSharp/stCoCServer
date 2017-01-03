using System;

namespace stClient.SpellChecker.Affixes
{
	internal abstract class Affix
	{
		protected string stripChars;
		protected string addChars;
		protected string conditionChars;


		/// <summary>
		/// .ctor
		/// </summary>
		public Affix ()
		{
		}




		/// <summary>
		/// 
		/// </summary>
		/// <param name="data">SFX N   0     en         [^ey]</param>
		/// <returns></returns>
		protected bool ParseData (string[] data)
		{
			if (data[3] == "0")
				return false;
			addChars = data[3];
			stripChars = (data[2] == "0") ? null : data[2];
			conditionChars= (data[4] == ".") ? null : data[4];
			if (conditionChars!= null && conditionChars.IndexOf ('[') != -1 && conditionChars.IndexOf (']') == -1)
				return false;			// invalid condition

			return true;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="word"></param>
		/// <returns>null, if can't be applied</returns>
		public abstract string ApplyReverse (string word);



		/// <summary>
		/// Checks only 2 types of conditions ( []-chars group must be 1st, '.' - any symbol):
		/// [abc]d.ef
		/// [^abc]d.ef
		/// The function is much faster than RegEx.
		/// </summary>
		/// <param name="word"></param>
		/// <param name="fromBeginning">whether to check the word from beginning or from end</param>
		/// <returns></returns>
		protected bool CheckCondition (string word, bool fromBeginning)
		{
			if (conditionChars == null)
				return true;
			if (String.IsNullOrEmpty (word))
				return false;

			int wordCheckPos;
			string condition = conditionChars;
			if (condition.StartsWith ("[^"))
			{
				// excluding chars group
				int p= condition.IndexOf (']');
				string group = condition.Substring (2, p - 2);
				condition = condition.Substring (p + 1);

				wordCheckPos = fromBeginning ? 0 : word.Length - condition.Length - 1;
				if (wordCheckPos< 0 || group.IndexOf (word[wordCheckPos]) != -1)
					return false;
				++wordCheckPos;
			}
			else if (condition.StartsWith ("["))
			{
				// including chars group
				int p = condition.IndexOf (']');
				string group = condition.Substring (1, p - 1);
				condition = condition.Substring (p + 1);

				wordCheckPos = fromBeginning ? 0 : word.Length - condition.Length - 1;
				if (wordCheckPos < 0 || group.IndexOf (word[wordCheckPos]) == -1)
					return false;
				++wordCheckPos;
			}
			else
			{
				wordCheckPos = fromBeginning ? 0 : word.Length - condition.Length;
			}

			// checking chars sequence
			int n = condition.Length;
			if (wordCheckPos < 0 || wordCheckPos + n > word.Length)
				return false;
			for (int i = 0; i < n; ++i)
			{
				if (condition[i] == '.')
					continue;				// any char
				if (word[wordCheckPos + i] != condition[i])
					return false;
			}

			return true;
		}




	}
}
