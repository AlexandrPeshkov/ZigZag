﻿namespace ZigZag
{
	/// <summary>
	/// Управление параметрами геймплея
	/// </summary>
	public class GamePlayService
	{
		private const float _baseSpeed = 5f;

		private float _speedBonus;

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

		public GamePlayService()
		{
			DifficultyLevel = 1;
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
		}

		public void UseLife()
		{
			Lifes--;
		}
	}
}