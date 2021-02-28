using UnityEngine;

namespace ZigZag
{
	/// <summary>
	/// Методы расширения камеры
	/// </summary>
	public static class CameraExtensions
	{
		/// <summary>
		/// Преокция мировых координат в экранные (с фиксом проекции, если объект позади камеры)
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static Vector2 WorldToScreenPointProjected(this Camera camera, Vector3 worldPos)
		{
			Transform cameraTransform = camera.transform;
			Vector3 cameraPosition = cameraTransform.position;

			Vector3 camNormal = cameraTransform.forward;
			Vector3 vectorFromCam = worldPos - cameraPosition;
			float camNormDot = Vector3.Dot(camNormal, vectorFromCam);
			if (camNormDot <= 0)
			{
				//если объект позадит камеры, сдвигаем его позицию вдоль вектора камеры
				Vector3 proj = (camNormal * camNormDot * 1.01f);
				worldPos = cameraPosition + (vectorFromCam - proj);
			}

			return RectTransformUtility.WorldToScreenPoint(camera, worldPos);
		}
	}
}