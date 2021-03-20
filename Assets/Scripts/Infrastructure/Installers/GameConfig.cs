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

		[Tooltip("Размер пула гемов")]
		public int GemPoolSize;

		[Space]
		[Header("Настройки платформ")]
		//public int PlatformPoolSize;

		[Tooltip("Длина стартовой прямой линии")]
		[Min(4)]
		public int FirstLineLength;

		[Tooltip("Расстояние от текущей платформы для скрытия")]
		public int TailLengthForHide;

		[Tooltip("Размер пула платформ")]
		public int PlatformPoolSize;
	}
}