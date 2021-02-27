using System;
using Zenject;
using ZigZag.Abstracts;

namespace ZigZag.Services
{
	public class GameStateService : IInitializable
	{
		public GameState State { get; private set; }

		/// <summary>
		/// Смена игрового состояния
		/// </summary>
		public event Action<GameState> GameStateChanged;

		//[Inject]
		//private void Construct()
		//{
		//}

		public void Initialize()
		{
			Start();
		}

		private void Start()
		{
			//TODO Продумать как запускать приложение не монобехом
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
			GameStateChanged?.Invoke(State);
		}
	}
}