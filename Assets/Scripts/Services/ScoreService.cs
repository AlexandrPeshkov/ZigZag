using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;

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

		public ScoreService(SignalBus signalBus)
		{
			ScoreTable = new List<int>(Enumerable.Repeat(0, _highScoreSize));

#if UNITY_EDITOR
			//ClearRecordTable();
#endif
			signalBus.Subscribe<GameStateSignal>(OnGameStateChanged);
			signalBus.Subscribe<PlatformCompleteSignal>(OnPlatformComplete);

			ReadScoreTable();
		}

		private void OnGameStateChanged(GameStateSignal signal)
		{
			switch (signal.GameState)
			{
				case GameState.Failed:
					{
						OnGameFailed();
						break;
					}
				case GameState.Reset:
					{
						OnGameRestarted();
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

		private void OnGameRestarted()
		{
			ResetScore();
		}

		private void OnPlatformComplete(PlatformCompleteSignal completeSignal)
		{
			AddPoints(completeSignal.Points);
		}

		private void ResetScore()
		{
			CurrentScore = 0;
			_isNewRecordSession = false;
		}

		private void AddPoints(int points)
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
				//work after Start()
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