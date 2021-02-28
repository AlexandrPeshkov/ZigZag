using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag.Infrastructure
{
	public class SoundManager : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _backgroundSound;

		[Inject]
		private void Construct(GameStateService stateService)
		{
			stateService.GameStateChanged += OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Run:
					{
						PlayBackground();
						break;
					}
				default:
					{
						StopBackground();
						break;
					}
			}
		}

		private void PlayBackground()
		{
			_backgroundSound.Play();
		}

		private void StopBackground()
		{
			_backgroundSound.Stop();
		}
	}
}