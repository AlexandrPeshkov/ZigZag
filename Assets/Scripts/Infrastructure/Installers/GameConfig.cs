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
		[Header("Вероятность бонуса очков")]
		public float PointsBonusProbability;

		[Header("Вероятность бонуса очков")]
		public float SpeedBonusProbability;

		[Space]
		[Header("Настройки производительности")]
		[Header("Размер пула платформ")]
		public int PlatformPoolSize;
	}
}