using UnityEngine;
using UnityEngine.UI;

namespace ZigZag.UI
{
	public class FadeableText : BaseFadeableElem
	{
		[SerializeField]
		private Text _text;

		protected override void SetContentAlpha(float alpha)
		{
			_text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
		}

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}