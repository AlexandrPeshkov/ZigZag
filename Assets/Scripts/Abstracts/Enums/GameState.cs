namespace ZigZag.Abstracts
{
	/// <summary>
	/// Игровое состояние
	/// </summary>
	public enum GameState
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		Init,

		/// <summary>
		/// Пауза перед стартом
		/// </summary>
		Pause,

		/// <summary>
		/// Выполнение игровой логики
		/// </summary>
		Runing,

		/// <summary>
		/// Прерывание игры
		/// </summary>
		Failing,

		/// <summary>
		/// Восстановлене игры
		/// </summary>
		Restore,

		/// <summary>
		/// Остановка геймплея (шарик упал)
		/// </summary>
		Failed,

		/// <summary>
		/// Перезапуск игры
		/// </summary>
		StartNewGame
	}
}