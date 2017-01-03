namespace stClient.SpellChecker
{
	public class SpellSuggestion
	{
		private readonly string text;


		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="text"></param>
		/// <param name="editDistance"></param>
		public SpellSuggestion (string text, int editDistance)
		{
			this.text = text;
			EditDistance = editDistance;
		}




		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get
			{
				return text;
			}
		}




		/// <summary>
		/// 
		/// </summary>
		public int EditDistance
		{
			get;
			private set;
		}

	}
}
