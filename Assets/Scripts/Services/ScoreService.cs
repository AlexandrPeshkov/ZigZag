﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag
{
	public class ScoreService
	{
		private bool _isNewRecordSession = false;

		private int _currentScore;

		private readonly PlayerPrefsStorage _prefsStorage;

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

		public ScoreService(GameStateService gameStateService, PlayerPrefsStorage playerPrefsStorage)
		{
			_prefsStorage = playerPrefsStorage;

#if DEBUG
			_prefsStorage.ClearRecordTable();
#endif
			ScoreTable = _prefsStorage.ReadScoreTable();

			gameStateService.GameStateChanged += OnGameStateChanged;
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
			ScoreTable = ScoreTable.OrderByDescending(x => x).ToList();

			_prefsStorage.WriteScoreTable(ScoreTable);
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
	}
}