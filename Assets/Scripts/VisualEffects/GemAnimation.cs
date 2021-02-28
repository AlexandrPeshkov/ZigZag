using UnityEngine;

namespace ZigZag
{
	public class GemAnimation : MonoBehaviour
	{
		//Flags
		[Header("Флаги")]
		public bool isAnimated = false;

		public bool isRotating = false;
		public bool isFloating = false;
		public bool isScaling = false;

		//Rotation
		[Header("Вращение")]
		public Vector3 rotationAngle;

		public float rotationSpeed;

		//Floating
		[Header("Смещение")]
		//Единичный вектор оси смещения
		public Vector3 floatingVector;

		public float floatSpeed;

		[Min(0)]
		public float floatHeight;

		private bool goingTo = true;

		private Vector3 startPos;

		//Scaling
		[Header("Масштаб")]
		public Vector3 startScale;

		public Vector3 endScale;

		public float scaleSpeed;
		public float scaleRate;

		private bool scalingUp = true;
		private float scaleTimer;

		private void Start()
		{
			startPos = transform.position;
		}

		private void FixedUpdate()
		{
			if (isAnimated)
			{
				Rotation();
				Floating();
				Scaling();
			}
		}

		private void Rotation()
		{
			if (isRotating)
			{
				transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
			}
		}

		private void Floating()
		{
			if (isFloating)
			{
				Vector3 currentfloatingVector = goingTo ? floatingVector : -floatingVector;

				float step = Time.deltaTime * floatSpeed;
				Vector3 floating = currentfloatingVector * step;
				Vector3 position = transform.position + floating;

				if (Mathf.Abs(position.y) - Mathf.Abs(startPos.y) > floatHeight)
				{
					goingTo = false;
				}

				if (position.y - startPos.y <= 0)
				{
					goingTo = true;
				}

				transform.Translate(floating);
			}
		}

		private void Scaling()
		{
			if (isScaling)
			{
				scaleTimer += Time.deltaTime;

				if (scalingUp)
				{
					transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
				}
				else if (!scalingUp)
				{
					transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
				}

				if (scaleTimer >= scaleRate)
				{
					if (scalingUp) { scalingUp = false; }
					else if (!scalingUp) { scalingUp = true; }
					scaleTimer = 0;
				}
			}
		}
	}
}