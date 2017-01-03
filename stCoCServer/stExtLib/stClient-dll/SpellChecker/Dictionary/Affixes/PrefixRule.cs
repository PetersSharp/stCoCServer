namespace stClient.SpellChecker.Affixes
{
	internal sealed class PrefixRule : AffixRule
	{

		/// <summary>
		/// .ctor
		/// </summary>
		public PrefixRule ()
		{
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="data">PFX U   0     un         .</param>
		/// <returns></returns>
		public override bool ParseAndAddAffix (string[] data)
		{
			Prefix p = Prefix.Parse (data);
			if (p == null)
				return false;

			affixes.Add (p);
			return true;
		}

	}
}
