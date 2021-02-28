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
		Run,

		/// <summary>
		/// Остановка геймплея (шарик упал)
		/// </summary>
		Failed,

		/// <summary>
		/// Перезапуск игры
		/// </summary>
		Reset
	}
}