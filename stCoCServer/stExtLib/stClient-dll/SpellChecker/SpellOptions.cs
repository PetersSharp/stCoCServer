namespace stClient.SpellChecker
{
	public class SpellOptions
	{
		/// <summary>
		/// .ctor
		/// </summary>
		public SpellOptions()
		{
			IgnoreUppercaseWords = true;
			CaseSensitive = true;
		}


		/// <summary>
		/// Whether to ignore words, which are written fully in uppercase.
		/// </summary>
		public bool IgnoreUppercaseWords
		{
			get;
			set;
		}



		/// <summary>
		/// Whether to ignore letters capitalization.
		/// </summary>
		public bool CaseSensitive
		{
			get;
			set;
		}
	}
}
