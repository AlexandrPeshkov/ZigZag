using System;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;

namespace ZigZag.Services
{
	public class GameStateService : MonoBehaviour
	{
		public GameState State { get; private set; }

		/// <summary>
		/// Смена игрового состояния
		/// </summary>
		public event Action<GameState> GameStateChanged;

		[Inject]
		private void Construct()
		{
		}

		private void Start()
		{
			ChangeState(GameState.Init);
			ChangeState(GameState.Pause);
		}

		public GameStateService(SignalBus signalBus)
		{
		}

		/// <summary>
		/// Перевести игру в другое состояние
		/// </summary>
		/// <param name="gameState"></param>
		public void ChangeState(GameState gameState)
		{
			State = gameState;
			GameStateChanged?.Invoke(State);
		}
	}
}