﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Services;

namespace ZigZag.UI
{
	public class LifeValueView : MonoBehaviour
	{
		[SerializeField]
		private Text _lifeValue;

		[SerializeField]
		private Image _heartIcon;

		[SerializeField]
		private Color _emptyColor;

		[SerializeField]
		private Color _activelColor;

		private GamePlayService _gamePlayService;

		[Inject]
		private void Construct(GamePlayService gamePlayService, GameStateService stateService)
		{
			_gamePlayService = gamePlayService;

			stateService.GameStateChanged += OnGameStateChanged;
			gamePlayService.LifesChanged += ShowLifes;
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Init:
					{
						ShowLifes(_gamePlayService.Lifes);
						break;
					}
			}
		}

		private void ShowLifes(int lifes)
		{
			_lifeValue.text = lifes.ToString();
			if (lifes > 0)
			{
				_heartIcon.color = _activelColor;
			}
			else
			{
				_heartIcon.color = _emptyColor;
			}
		}
	}
}