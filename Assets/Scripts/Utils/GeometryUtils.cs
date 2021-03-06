using System;
using UnityEngine;

namespace ZigZag.Utils
{
	public static class GeometryUtils
	{
		public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
		{
			//Func<float, float> f = x => -4f * height * x * x + 4f * height * x;

			float y = -1f * height * t * t + 1f * height * t;
			//float y = f(t);
			var mid = Vector3.Lerp(start, end, t);

			return new Vector3(mid.x, y + Mathf.Lerp(start.y, end.y, t), mid.z);
		}
	}
}