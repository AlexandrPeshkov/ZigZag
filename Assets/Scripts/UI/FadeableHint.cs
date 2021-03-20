using UnityEngine;
using UnityEngine.UI;

namespace ZigZag.UI
{
	public class FadeableHint : BaseFadeableElem
	{
		[SerializeField]
		private Text _text;

		[SerializeField]
		private Image _icon;

		protected override void SetContentAlpha(float alpha)
		{
			_text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
			_icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, alpha);
		}

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}