namespace stClient.SpellChecker
{
	/// <summary>
	/// Weights of different editing actions.
	/// </summary>
	public class EditDistanceWeights
	{
		public int ReplaceCharWeight = 1;
		public int InsertCharWeight = 1;
		public int SwapCharWeight = 1;
		public int DeleteCharWeight = 1;
	}
}
