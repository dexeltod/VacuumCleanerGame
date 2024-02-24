namespace Sources.Presentation.UI
{
	public static class Normalizer
	{
		public static float Normalize(
			float topValue,
			float newScore,
			float currentMaxScore
		) =>
			topValue / currentMaxScore * newScore;
	}
}