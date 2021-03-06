using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag
{
	public class ScoreService
	{
		private const int _highScoreSize = 3;

		private const string _highScoreKey = "HighScore_";

		private bool _isNewRecordSession = false;

		private int _currentScore;

		public List<int> ScoreTable { get; private set; }

		public int CurrentScore
		{
			get => _currentScore;

			private set
			{
				_currentScore = value;
				ScoreChanged?.Invoke(_currentScore);
			}
		}

		public event Action NewRecord;

		public event Action<int> ScoreChanged;

		public ScoreService(GameStateService gameStateService)
		{
			ScoreTable = new List<int>(Enumerable.Repeat(0, _highScoreSize));

#if UNITY_EDITOR
			//ClearRecordTable();
#endif
			gameStateService.GameStateChanged += OnGameStateChanged;

			ReadScoreTable();
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Failed:
					{
						OnGameFailed();
						break;
					}
				case GameState.StartNewGame:
					{
						ResetScore();
						break;
					}
				default: break;
			}
		}

		private void OnGameFailed()
		{
			ScoreTable.Add(CurrentScore);
			WriteScoreTable();
		}

		private void ResetScore()
		{
			CurrentScore = 0;
			_isNewRecordSession = false;
		}

		public void AddPoints(int points)
		{
			CurrentScore += points;
			ScoreChanged?.Invoke(CurrentScore);

			if (_isNewRecordSession == false)
			{
				bool isRecord = ScoreTable[0] < CurrentScore;

				if (isRecord)
				{
					NewRecord?.Invoke();
					_isNewRecordSession = true;
				}
			}
		}

		private void ReadScoreTable()
		{
			for (var i = 0; i < _highScoreSize; i++)
			{
				var scoreValue = PlayerPrefs.GetInt($"{_highScoreKey}{i}", -1);
				if (scoreValue > 0)
				{
					ScoreTable[i] = scoreValue;
				}
			}

			ScoreTable = ScoreTable.OrderByDescending(x => x).ToList();
		}

		private void WriteScoreTable()
		{
			ScoreTable = ScoreTable.OrderByDescending(x => x).Take(_highScoreSize).ToList();

			for (var i = 0; i < ScoreTable.Count; i++)
			{
				PlayerPrefs.SetInt($"{_highScoreKey}{i}", ScoreTable[i]);
				PlayerPrefs.Save();
			}
		}

#if UNITY_EDITOR

		private void ClearRecordTable()
		{
			for (var i = 0; i < ScoreTable.Count; i++)
			{
				PlayerPrefs.SetInt($"{_highScoreKey}{i}", 0);
				PlayerPrefs.Save();
			}
		}

#endif
	}
}