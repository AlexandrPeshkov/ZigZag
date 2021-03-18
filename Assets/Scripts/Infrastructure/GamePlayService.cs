using System;

namespace ZigZag
{
	/// <summary>
	/// Управление параметрами геймплея
	/// </summary>
	public class GamePlayService
	{
		private const float _baseSpeed = 5f;

		private float _speedBonus;

		private readonly PlayerPrefsStorage _prefsStorage;

		/// <summary>
		/// Скорость сферы
		/// </summary>
		public float Speed => _baseSpeed * DifficultyLevel + _speedBonus;

		/// <summary>
		/// Текущая сложность
		/// </summary>
		public int DifficultyLevel { get; private set; }

		/// <summary>
		/// Доп.жизни
		/// </summary>
		public int Lifes { get; private set; }

		/// <summary>
		/// Число жизней измененео
		/// </summary>
		public event Action<int> LifesChanged;

		public GamePlayService(PlayerPrefsStorage prefsStorage)
		{
			_prefsStorage = prefsStorage;
			DifficultyLevel = 1;

			Lifes = _prefsStorage.ReadLifes();
		}

		/// <summary>
		/// Добавить бонус скорости
		/// </summary>
		/// <param name="speedBonus"></param>
		public void ChangeSpeedBonus(float speedBonus)
		{
			_speedBonus = speedBonus;
		}

		/// <summary>
		/// Повысить сложность
		/// </summary>
		public void IncreaseDifficulty()
		{
			DifficultyLevel++;
		}

		public void AddLife()
		{
			Lifes++;
			_prefsStorage.WriteLifes(Lifes);
			LifesChanged?.Invoke(Lifes);
		}

		public void UseLife()
		{
			Lifes--;
			_prefsStorage.WriteLifes(Lifes);
			LifesChanged?.Invoke(Lifes);
		}
	}
}