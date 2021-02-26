using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;

namespace ZigZag.Services
{
	public class GameStateService : MonoBehaviour
	{
		private SignalBus _signalBus;

		public GameState State { get; private set; }

		[Inject]
		private void Construct(SignalBus signalBus)
		{
			_signalBus = signalBus;
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
			_signalBus.Fire(new GameStateSignal(State));
		}
	}
}