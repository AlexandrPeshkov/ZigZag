using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag
{
	/// <summary>
	/// Диалог завершения игры
	/// </summary>
	public class FinishGameDialog : MonoBehaviour
	{
		[SerializeField]
		private GameObject _content;

		[SerializeField]
		private Button _newGameButton;

		[SerializeField]
		private Button _continueGameButton;

		[SerializeField]
		private Image _heartIcon;

		[SerializeField]
		private Color _activeColor;

		[SerializeField]
		private Color _disabledColor;

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
			bool hasLife = _gamePlayService.Lifes > 0;
			_continueGameButton.interactable = hasLife;

			_heartIcon.color = hasLife ? _activelColor : _emptyColor;

			_content.SetActive(true);
		}

		private void Hide()
		{
			if (_content.activeSelf == true)
			{
				_content.SetActive(false);
			}
		}
	}
}