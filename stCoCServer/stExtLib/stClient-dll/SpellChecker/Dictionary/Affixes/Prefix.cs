namespace stClient.SpellChecker.Affixes
{

	internal class Prefix: Affix
	{

		/// <summary>
		/// .ctor
		/// </summary>
		public Prefix ()
		{
		}



		/// <summary>
		/// Parses strings like:
		/// PFX A   0     re         .
		/// </summary>
		/// <param name="data"></param>
		/// <returns>null if parsing failes</returns>
		public static Prefix Parse (string[] data)
		{
			if (data.Length != 5)
				return null;
			if (data[0] != "PFX")
				return null;

			Prefix pfx = new Prefix ();
			if (!pfx.ParseData (data))
				return null;

			return pfx;
		}



		/// <summary>
		/// Unapplies (removes) the prefix from the passed word.
		/// </summary>
		/// <param name="word"></param>
		/// <returns>null, if can't be applied</returns>
		public override string ApplyReverse (string word)
		{
			// remove "add chars"
			if (!word.StartsWith (addChars))
				return null;
			word = word.Remove (0, addChars.Length);

			// add "strip chars"
			if (stripChars != null)
				word = stripChars+ word;

			// check condition
			if (!CheckCondition (word, true))
				return null;

			return word;
		}


	}
}
