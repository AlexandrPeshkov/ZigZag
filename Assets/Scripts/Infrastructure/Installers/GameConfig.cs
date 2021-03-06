using System;
using UnityEngine;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// Настройки приложения
	/// </summary>
	[Serializable]
	public class GameConfig
	{
		[Header("Настройки бонусов")]
		[Tooltip("Вероятность бонуса очков")]
		[Range(0, 1)]
		public float PointsGemChance;

		[Tooltip("Вероятность бонуса очков")]
		[Range(0, 1)]
		public float SpeedGemChance;

		[Tooltip("Вероятность бонуса жизней")]
		[Range(0, 1)]
		public float LifeGemChance;

		[Space]
		[Header("Настройки производительности")]
		[Tooltip("Вероятность бонуса очков")]
		public int PlatformPoolSize;

		[Tooltip("Длина стартовой прямой линии")]
		[Min(4)]
		public int firstLineLength;
	}
}