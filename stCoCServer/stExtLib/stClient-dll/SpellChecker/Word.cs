
namespace stClient.SpellChecker
{
	public class Word
	{
		private readonly int startIndex;
		private readonly int endIndex;
		private readonly string srcString;
		private string text;



		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="endIndex"></param>
		/// <param name="srcString"></param>
		public Word (int startIndex, int endIndex, string srcString)
		{
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.srcString= srcString;
		}



		/// <summary>
		/// Index of the first character of the word.
		/// </summary>
		public int StartIndex
		{
			get
			{
				return startIndex;
			}
		}



		/// <summary>
		/// Index of the last character of the word.
		/// </summary>
		public int EndIndex
		{
			get
			{
				return endIndex;
			}
		}



		/// <summary>
		/// Word length.
		/// </summary>
		public int Length
		{
			get
			{
				return endIndex-startIndex + 1;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get
			{
				if (text== null)
					text = srcString.Substring (startIndex, Length);

				return text;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;

			Word w2 = obj as Word;
			if (w2== null)
				return false;

			if (w2.startIndex == startIndex && w2.endIndex == endIndex && w2.srcString == srcString)
				return true;

			return false;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}





	}
}
