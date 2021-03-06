using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZigZag.UI
{
	public class ScoreValueView : MonoBehaviour
	{
		[SerializeField]
		private Text _scoreValue;

		[Inject]
		private void Construct(ScoreService scoreService)
		{
			scoreService.ScoreChanged += (score) => _scoreValue.text = score.ToString();
		}
	}
}