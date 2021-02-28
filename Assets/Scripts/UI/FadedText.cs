using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZigZag.UI
{
	public class FadedText : MonoBehaviour
	{
		[SerializeField]
		private Text _text;

		private const float _fadeTime = 1.5f;

		[SerializeField]
		private RectTransform _rectTranform;

		private Camera _mainCamera;

		private Canvas _mainCanvas;

		private RectTransform _mainCanvasRect;

		private Vector2 _offset;

		[Inject]
		private void Construct(
			[Inject(Id = DiConstants._mainCamera)] Camera mainCamera,
			[Inject(Id = DiConstants._mainCanvas)] Canvas mainCanvas)
		{
			_mainCamera = mainCamera;
			_mainCanvas = mainCanvas;
			_mainCanvasRect = _mainCanvas.GetComponent<RectTransform>();

			_offset = _rectTranform.sizeDelta / -2f;
		}

		public virtual void Show(Vector3 gemWorldPos, string text)
		{
			_rectTranform.SetParent(_mainCanvasRect);
			_text.text = text;
			_offset = _rectTranform.sizeDelta / -2f;

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
				transform.position += Vector3.up;
				timer += Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
			while (timer < _fadeTime);

			Destroy(this.gameObject);
		}
	}
}