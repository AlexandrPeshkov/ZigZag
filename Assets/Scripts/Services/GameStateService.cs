using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;

namespace ZigZag.Services
{
	public class GameStateService : MonoBehaviour
	{
		private readonly SignalBus _signalBus;

		public GameState State { get; private set; }

		public GameStateService(SignalBus signalBus)
		{
			_signalBus = signalBus;
			ChangeState(GameState.Init);
			ChangeState(GameState.Pause);
		}

		/// <summary>
		/// Перевести игру в другое состояние
		/// </summary>
		/// <param name="gameState"></param>
		public void ChangeState(GameState gameState)
		{
			State = gameState;
			_signalBus.Fire(new GameStateSignal(State));
		}
	}
}