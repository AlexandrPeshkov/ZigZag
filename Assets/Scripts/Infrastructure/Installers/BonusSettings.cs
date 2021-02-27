using System;
using UnityEngine;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// Настройки приложения
	/// </summary>
	[Serializable]
	public class BonusSettings
	{
		[Header("Вероятность бонуса очков")]
		public float PointBonusProbability;
	}
}