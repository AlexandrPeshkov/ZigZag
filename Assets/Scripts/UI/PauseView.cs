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

		[Inject]
		private void Construct(SignalBus signalBus, GameStateService stateService)
		{
			_stateService = stateService;
			signalBus.Subscribe<GameStateSignal>(OnGameStateChanged);
		}

		private void OnGameStateChanged(GameStateSignal signal)
		{
			switch (signal.GameState)
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

		private void Update()
		{
			if (_stateService.State == GameState.Pause)
			{
				//произошел тап
				if (Input.GetMouseButtonUp(0))
				{
					_content.SetActive(false);

					StartCoroutine(ShowCountdown());
				}
			}
		}

		private IEnumerator ShowCountdown()
		{
			for (var i = 0; i < _countdownElems.Count; i++)
			{
				_countdownElems[i]?.SetActive(true);
				if (i > 1)
				{
					_countdownElems[i - 1]?.SetActive(false);
				}

				yield return new WaitForSecondsRealtime(1);
			}

			_countdownElems[_countdownElems.Count - 1].SetActive(false);
		}
	}
}