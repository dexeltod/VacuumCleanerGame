namespace Sources.Utils
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