using Zenject;

namespace ZigZag
{
	/// <summary>
	/// Фабрика эффектов
	/// </summary>
	/// <typeparam name="TEffect">эффект</typeparam>
	public interface IEffectFactory<TEffect> : IFactory<TEffect> where TEffect : IEffect
	{
	}
}