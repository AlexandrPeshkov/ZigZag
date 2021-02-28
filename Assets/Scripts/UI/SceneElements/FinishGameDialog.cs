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

		//[SerializeField]
		//private GameObject _content;

		private GameStateService _stateService;

		[Inject]
		private void Construct(GameStateService stateService)
		{
			_stateService = stateService;

			_newGameButton.onClick.AddListener(OnNewGameClick);

			stateService.GameStateChanged += OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Failed:
					{
						gameObject.SetActive(true);
						break;
					}
				default:
					{
						if (gameObject.activeSelf == true)
						{
							gameObject.SetActive(false);
						}
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