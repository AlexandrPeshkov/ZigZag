using ZigZag.Abstracts;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// Сигнал перехода игры в другое состояние
	/// </summary>
	public class GameStateSignal
	{
		/// <summary>
		/// Состояние игры
		/// </summary>
		public GameState GameState { get; private set; }

		public GameStateSignal(GameState state) => (GameState) = (state);
	}
}