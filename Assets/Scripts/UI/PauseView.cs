using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag.UI
{
	public class PauseView : MonoBehaviour
	{
		private GameStateService _stateService;

		[SerializeField]
		private GameObject _content;

		[SerializeField]
		private List<GameObject> _countdownElems;

		[SerializeField]
		private Text _tapText;

		private InputHandler _inputHandler;

		/// <summary>
		/// Показ отсчета запущен
		/// </summary>
		private bool _countDownStarted = false;

		[Inject]
		private void Construct(GameStateService stateService, InputHandler inputHandler)
		{
			_stateService = stateService;
			_inputHandler = inputHandler;

			stateService.GameStateChanged += OnGameStateChanged;
			_inputHandler.LeftMouseButtonUp += OnLeftMouseButtonUp;
		}

		private void OnLeftMouseButtonUp()
		{
			if (_stateService.State == GameState.Pause)
			{
				//произошел тап
				if (Input.GetMouseButtonUp(0) && _countDownStarted == false)
				{
					_countDownStarted = true;
					_content.SetActive(false);

					StartCoroutine(ShowCountdown());
				}
			}
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Pause:
					{
						OnGamePause();
						break;
					}
			}
		}

		private void OnGamePause()
		{
			_content.SetActive(true);
		}

		private IEnumerator ShowCountdown()
		{
			for (var i = 0; i < _countdownElems.Count; i++)
			{
				_countdownElems[i]?.SetActive(true);

				yield return new WaitForSecondsRealtime(0.7f);

				_countdownElems[i]?.SetActive(false);
			}

			_stateService.ChangeState(GameState.Run);
			_countDownStarted = false;
		}
	}
}