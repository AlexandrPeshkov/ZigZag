namespace ZigZag
{
	/// <summary>
	/// Эффект геймплея
	/// </summary>
	public interface IEffect
	{
		/// <summary>
		/// Применить
		/// </summary>
		void Apply();

		/// <summary>
		/// Отменить
		/// </summary>
		void Cancel();

		/// <summary>
		/// Тип жизненного цикла
		/// </summary>
		EffectLifecycle EffectLifecycle { get; }
	}
}