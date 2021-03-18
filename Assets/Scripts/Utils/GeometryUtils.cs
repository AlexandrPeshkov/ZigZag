using System;
using UnityEngine;

namespace ZigZag.Utils
{
	public static class GeometryUtils
	{
		/// <summary>
		/// Получение точки на параболе
		/// </summary>
		/// <param name="start">Стартовая точка</param>
		/// <param name="end">Конечная точка</par
		/// <param name="height">Высота параболы</param>
		/// <param name="t">Коэф. интерполяции</param>
		/// <returns></returns>
		public static Vector3 PointOnParabola(Vector3 start, Vector3 end, float height, float t)
		{
			float y = (height * t) * (-t + 1);

			var mid = Vector3.Lerp(start, end, t);

			return new Vector3(mid.x, y + Mathf.Lerp(start.y, end.y, t), mid.z);
		}
	}
}