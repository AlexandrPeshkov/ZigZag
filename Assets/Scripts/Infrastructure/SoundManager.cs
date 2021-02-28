using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// Сервис управление звуками
	/// </summary>
	public class SoundManager : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _backgroundAudioSource;

		[SerializeField]
		private AudioSource _effectAudioSource;

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
			_backgroundAudioSource.Play();
		}

		private void StopBackground()
		{
			_backgroundAudioSource.Stop();
		}

		public void PlayEffectSound(AudioClip effectAudio)
		{
			_effectAudioSource.clip = effectAudio;
			_effectAudioSource.Play();
		}
	}
}