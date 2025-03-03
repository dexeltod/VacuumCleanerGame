using UnityEngine;

namespace Sources.Utils.SceneDebugExcentions
{
	public static class DebugExtensions
	{
		/// <summary>
		///     Визуализирует BoxCast в сцене.
		/// </summary>
		/// <param name="center">Центр BoxCast.</param>
		/// <param name="halfExtents">Половина размеров BoxCast.</param>
		/// <param name="orientation">Ориентация BoxCast.</param>
		/// <param name="direction">Направление BoxCast.</param>
		/// <param name="distance">Длина BoxCast.</param>
		/// <param name="color">Цвет визуализации.</param>
		public static void DrawBoxCast(
			Vector3 center,
			Vector3 halfExtents,
			Quaternion orientation,
			Vector3 direction,
			float distance,
			Color color)
		{
			// Рисуем начальный бокс
			DrawBox(center, halfExtents, orientation, color);

			// Рисуем конечный бокс
			Vector3 endPosition = center + direction * distance;
			DrawBox(endPosition, halfExtents, orientation, color);

			// Рисуем линии, соединяющие углы начального и конечного боксов
			Vector3[] startCorners = GetBoxCorners(center, halfExtents, orientation);
			Vector3[] endCorners = GetBoxCorners(endPosition, halfExtents, orientation);

			for (var i = 0; i < 4; i++)
			{
				Debug.DrawLine(startCorners[i], endCorners[i], color);
				Debug.DrawLine(startCorners[i + 4], endCorners[i + 4], color);
				Debug.DrawLine(startCorners[i], startCorners[i + 4], color);
				Debug.DrawLine(endCorners[i], endCorners[i + 4], color);
			}
		}

		/// <summary>
		///     Рисует бокс в сцене.
		/// </summary>
		/// <param name="center">Центр бокса.</param>
		/// <param name="halfExtents">Половина размеров бокса.</param>
		/// <param name="orientation">Ориентация бокса.</param>
		/// <param name="color">Цвет визуализации.</param>
		private static void DrawBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, Color color)
		{
			Vector3[] corners = GetBoxCorners(center, halfExtents, orientation);

			// Рисуем линии между углами бокса
			for (var i = 0; i < 4; i++)
			{
				Debug.DrawLine(corners[i], corners[(i + 1) % 4], color);
				Debug.DrawLine(corners[i + 4], corners[(i + 1) % 4 + 4], color);
				Debug.DrawLine(corners[i], corners[i + 4], color);
			}
		}

		/// <summary>
		///     Возвращает углы бокса.
		/// </summary>
		/// <param name="center">Центр бокса.</param>
		/// <param name="halfExtents">Половина размеров бокса.</param>
		/// <param name="orientation">Ориентация бокса.</param>
		/// <returns>Массив из 8 углов бокса.</returns>
		private static Vector3[] GetBoxCorners(Vector3 center, Vector3 halfExtents, Quaternion orientation)
		{
			var corners = new Vector3[8];

			// Локальные координаты углов бокса
			Vector3[] localCorners =
			{
				new(-halfExtents.x, -halfExtents.y, -halfExtents.z),
				new(halfExtents.x, -halfExtents.y, -halfExtents.z),
				new(halfExtents.x, -halfExtents.y, halfExtents.z),
				new(-halfExtents.x, -halfExtents.y, halfExtents.z),
				new(-halfExtents.x, halfExtents.y, -halfExtents.z),
				new(halfExtents.x, halfExtents.y, -halfExtents.z),
				new(halfExtents.x, halfExtents.y, halfExtents.z),
				new(-halfExtents.x, halfExtents.y, halfExtents.z)
			};

			// Преобразуем локальные координаты в мировые с учетом ориентации
			for (var i = 0; i < 8; i++) corners[i] = center + orientation * localCorners[i];

			return corners;
		}
	}
}
