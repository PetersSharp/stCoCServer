namespace stClient.SpellChecker.Affixes
{
	internal sealed class SuffixRule : AffixRule
	{


		/// <summary>
		/// .ctor
		/// </summary>
		public SuffixRule ()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="data">
		///		SFX V   0     ive        [^e]
		/// </param>
		/// <returns></returns>
		public override bool ParseAndAddAffix (string[] data)
		{
			Suffix s = Suffix.Parse (data);
			if (s == null)
				return false;
			
			affixes.Add (s);
			return true;
		}

	}
}
