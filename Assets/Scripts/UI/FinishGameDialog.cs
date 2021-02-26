using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;
using ZigZag.UI;

namespace ZigZag
{
	public class FinishGameDialog : MonoBehaviour
	{
		[SerializeField]
		private Button _newGameButton;

		[SerializeField]
		private GameObject _content;

		private SignalBus _signalBus;

		private GameStateService _stateService;

		[Inject]
		private void Construct(SignalBus signalBus, GameStateService stateService)
		{
			_signalBus = signalBus;
			_stateService = stateService;

			_newGameButton.onClick.AddListener(OnNewGameClick);

			_signalBus.Subscribe<GameStateSignal>(OnGameStateChanged);
		}

		private void OnGameStateChanged(GameStateSignal signal)
		{
			switch (signal.GameState)
			{
				case GameState.Failed:
					{
						_content.gameObject.SetActive(true);
						break;
					}
				default:
					{
						_content.gameObject.SetActive(false);
						break;
					}
			}
		}

		private void OnNewGameClick()
		{
			_stateService.ChangeState(GameState.Reset);
			_stateService.ChangeState(GameState.Pause);
		}
	}
}