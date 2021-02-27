using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag.UI
{
	/// <summary>
	/// Таблица рекордов
	/// </summary>
	public class RecordTable : MonoBehaviour
	{
		[SerializeField]
		private Text _first;

		[SerializeField]
		private Text _second;

		[SerializeField]
		private Text _third;

		private ScoreService _scoreService;

		[Inject]
		private void Construct(ScoreService scoreService, GameStateService gameStateService)
		{
			_scoreService = scoreService;

			gameStateService.GameStateChanged += OnGameStateChanged;
		}

		public void Show()
		{
			_first.text = _scoreService.ScoreTable[0].ToString();
			_second.text = _scoreService.ScoreTable[1].ToString();
			_third.text = _scoreService.ScoreTable[2].ToString();

			gameObject.SetActive(true);
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Failed:
					{
						Show();
						break;
					}
				default:
					{
						gameObject.SetActive(false);
						break;
					}
			}
		}
	}
}