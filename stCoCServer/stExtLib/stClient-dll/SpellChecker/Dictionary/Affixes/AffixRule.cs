using System.Collections.Generic;

namespace stClient.SpellChecker.Affixes
{
	/// <summary>
	/// 
	/// </summary>
	internal abstract class AffixRule
	{
		protected char name;
		protected bool canCombine;
		protected List<Affix> affixes;



		/// <summary>
		/// .ctor
		/// </summary>
		public AffixRule ()
		{
			affixes = new List<Affix> ();
		}



		/// <summary>
		/// 
		/// </summary>
		public char Name
		{
			get
			{
				return name;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public bool CanCombine
		{
			get
			{
				return canCombine;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public List<Affix> Affixes
		{
			get
			{
				return affixes;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="data">like:
		/// PFX A Y 1
		/// </param>
		/// <returns>null if parsing failed</returns>
		public static AffixRule Parse (string[] data)
		{
			if (data.Length != 4)
				return null;

			// type
			AffixRule r;
			if (data[0] == "SFX")
				r = new SuffixRule ();
			else if (data[0] == "PFX")
				r = new PrefixRule ();
			else
				return null;

			// name
			if (data[1].Length!= 1)
				return null;
			r.name = data[1][0];

			// can combine
			r.canCombine = (data[2] == "Y");

			return r;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public abstract bool ParseAndAddAffix (string[] data);



	}
}
