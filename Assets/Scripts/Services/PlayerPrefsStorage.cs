using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZigZag
{
	/// <summary>
	/// Управление хранилищем пользовательских данных
	/// </summary>
	public class PlayerPrefsStorage
	{
		private const string _highScoreKey = "HighScore_";

		private const string _lifesKey = "Lifes_";

		private const int _highScoreSize = 3;

		public List<int> ReadScoreTable()
		{
			int[] scoreTable = new int[_highScoreSize];

			for (var i = 0; i < _highScoreSize; i++)
			{
				var scoreValue = PlayerPrefs.GetInt($"{_highScoreKey}{i}", -1);
				if (scoreValue > 0)
				{
					scoreTable[i] = scoreValue;
				}
			}

			return scoreTable.OrderByDescending(x => x).ToList();
		}

		public void WriteScoreTable(List<int> scoreTable)
		{
			scoreTable = scoreTable.OrderByDescending(x => x).Take(_highScoreSize).ToList();

			for (var i = 0; i < scoreTable.Count; i++)
			{
				PlayerPrefs.SetInt($"{_highScoreKey}{i}", scoreTable[i]);
				PlayerPrefs.Save();
			}
		}

		public void ClearRecordTable()
		{
			for (var i = 0; i < _highScoreSize; i++)
			{
				PlayerPrefs.SetInt($"{_highScoreKey}{i}", 0);
				PlayerPrefs.Save();
			}
		}

		public int ReadLifes()
		{
			return PlayerPrefs.GetInt(_lifesKey, 0);
		}

		public void WriteLifes(int lifes)
		{
			PlayerPrefs.SetInt(_lifesKey, lifes);
			PlayerPrefs.Save();
		}
	}
}