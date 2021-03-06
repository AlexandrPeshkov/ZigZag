using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag
{
	public class FinishGameDialog : MonoBehaviour
	{
		[SerializeField]
		private Button _newGameButton;

		[SerializeField]
		private Button _continueGameButton;

		private GameStateService _stateService;

		private GamePlayService _gamePlayService;

		[Inject]
		private void Construct(GameStateService stateService, GamePlayService gamePlayService)
		{
			_stateService = stateService;
			_gamePlayService = gamePlayService;

			_newGameButton.onClick.AddListener(OnNewGameClick);
			_continueGameButton.onClick.AddListener(OnContinueClick);

			stateService.GameStateChanged += OnGameStateChanged;
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
						Hide();
						break;
					}
			}
		}

		private void OnNewGameClick()
		{
			_stateService.ChangeState(GameState.StartNewGame);
			_stateService.ChangeState(GameState.Pause);
		}

		private void OnContinueClick()
		{
			_gamePlayService.UseLife();
			_stateService.ChangeState(GameState.Restore);
		}

		private void Show()
		{
			_continueGameButton.interactable = _gamePlayService.Lifes > 0;

			gameObject.SetActive(true);
		}

		private void Hide()
		{
			if (gameObject.activeSelf == true)
			{
				gameObject.SetActive(false);
			}
		}
	}
}