using stClient.SpellChecker;
using stClient.SpellChecker.SuggestAlgorithms;

namespace stClient.SpellChecker.Affixes
{
	internal class Suffix: Affix
	{

		/// <summary>
		/// .ctor
		/// </summary>
		public Suffix ()
		{
		}



		/// <summary>
		/// Parses strings like:
		/// SFX N   0     en         [^ey]
		/// </summary>
		/// <param name="data"></param>
		/// <returns>null if parsing failes</returns>
		public static Suffix Parse (string[] data)
		{
			if (data.Length != 5)
				return null;
			if (data[0] != "SFX")
				return null;

			Suffix sfx = new Suffix ();
			if (!sfx.ParseData (data))
				return null;

			return sfx;
		}



		/// <summary>
		/// Unapplies (removes) the suffix from the passed word.
		/// </summary>
		/// <param name="word"></param>
		/// <returns>null, if can't be applied</returns>
		public override string ApplyReverse (string word)
		{
			// remove "add chars"
            if (!TextTools.EndsWith(word, addChars))
				return null;

			word = word.Remove (word.Length - addChars.Length);

			// add "strip chars"
			if (stripChars != null)
				word += stripChars;

			// check condition
			if (!CheckCondition (word, false))
				return null;

			return word;
		}


	}
}
