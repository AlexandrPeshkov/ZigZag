using System.Collections;
using UnityEngine;
using Zenject;

namespace ZigZag.UI
{
	/// <summary>
	/// Исчезающий текст
	/// </summary>
	public abstract class BaseFadeableElem : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _rectTranform;

		[SerializeField]
		private float _fadeTime = 1.5f;

		[SerializeField]
		private float _speed = 1.5f;

		private Vector3 _step;

		private Vector2 _offset;

		private Camera _mainCamera;

		private RectTransform _mainCanvasRect;

		[Inject]
		private void Construct(
			[Inject(Id = DiConstants._mainCamera)] Camera mainCamera,
			[Inject(Id = DiConstants._mainCanvas)] Canvas mainCanvas)
		{
			_mainCamera = mainCamera;
			_mainCanvasRect = mainCanvas.GetComponent<RectTransform>();

			_offset = _rectTranform.sizeDelta;
			_step = Vector3.up * _speed;
		}

		public virtual void Show(Vector3 gemWorldPos)
		{
			_rectTranform.SetParent(_mainCanvasRect);

			//_offset = _rectTranform.sizeDelta / -2f;

			Vector2 screenPos = _mainCamera.WorldToScreenPointProjected(gemWorldPos);
			screenPos += _offset;

			if (screenPos.x > 0 && screenPos.y > 0
				&& screenPos.x < _mainCanvasRect.sizeDelta.x
				&& screenPos.y < _mainCanvasRect.sizeDelta.y)
			{
				_rectTranform.position = screenPos;
			}

			StartCoroutine(Fading());
		}

		private IEnumerator Fading()
		{
			float timer = 0f;
			do
			{
				transform.position += _step;
				float alpha = 1f;
				float alphaConstTime = _fadeTime * 0.6f;
				float alphaFadeTime = _fadeTime - alphaConstTime;

				if (timer > alphaConstTime)
				{
					alpha = 1f - (timer - alphaConstTime) / (alphaFadeTime);
				}
				SetContentAlpha(alpha);
				timer += Time.deltaTime;

				yield return new WaitForFixedUpdate();
			}
			while (timer < _fadeTime);

			Destroy(this.gameObject);
		}

		protected abstract void SetContentAlpha(float alpha);
	}
}