using System.Collections;
using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;
using ZigZag.Utils;

namespace ZigZag
{
	public class SphereController : MonoBehaviour
	{
		public const string _objectName = nameof(SphereController);

		private const float _fallingHeight = 15f;

		private Vector3 _currentDirection = Vector3.forward;

		private Vector3 _startPosition;

		private float _sphereRadius;

		private Vector3 _radiusOffset;

		[SerializeField]
		private Transform _transform;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Platform _platformPrefab;

		private GameStateService _gameStateService;

		private InputHandler _inputHandler;

		private GamePlayService _gamePlay;

		private PlatformManager _platformManager;

		[Inject]
		private void Construct(GameStateService gameState, InputHandler inputHandler, GamePlayService gamePlay, PlatformManager platformManager)
		{
			_gameStateService = gameState;
			_inputHandler = inputHandler;
			_gamePlay = gamePlay;
			_platformManager = platformManager;

			gameState.GameStateChanged += OnGameStateChanged;

			_inputHandler.LeftMouseButtonUp += OnLeftMouseButtonUp;

			var platformHeight = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * _platformPrefab.GetComponent<Transform>().localScale.y * 0.5f;
			_sphereRadius = this.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * this.GetComponent<Transform>().localScale.y * 0.5f;

			_startPosition = new Vector3(0, platformHeight + _sphereRadius, 0);
			_radiusOffset = new Vector3(0, _sphereRadius, 0);
		}

		private void OnLeftMouseButtonUp()
		{
			if (_gameStateService.State == GameState.Runing)
			{
				_currentDirection = _currentDirection == Vector3.left ? Vector3.forward : Vector3.left;
			}
		}

		private void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.Pause:
					{
						OnGamePause();
						break;
					}
				case GameState.Failed:
					{
						OnGameFailed();
						break;
					}
				case GameState.Restore:
					{
						OnGameRestore();
						break;
					}
			}
		}

		private void FixedUpdate()
		{
			if (_gameStateService.State == GameState.Runing)
			{
				if (CheckFail())
				{
					StartCoroutine(FailAnimation());
				}
				else
				{
					Move();
				}
			}
		}

		private IEnumerator FailAnimation()
		{
			_gameStateService.ChangeState(GameState.Failing);

			float t = 0;
			Vector3 startPos = _transform.position;
			Vector3 endPos = _transform.position + Vector3.down * _fallingHeight + _currentDirection * _gamePlay.Speed;

			while (t <= 1f)
			{
				_transform.position = GeometryUtils.PointOnParabola(startPos, endPos, _fallingHeight, t);
				t += Time.deltaTime * 1f;
				yield return null;
			}
			_gameStateService.ChangeState(GameState.Failed);
		}

		private bool CheckFail()
		{
			return !_platformManager.IsInPlatforms(new Vector2(_transform.position.x, _transform.position.z));
		}

		private void Move()
		{
			_transform.position += _gamePlay.Speed * Time.deltaTime * _currentDirection;
		}

		private void OnGamePause()
		{
			_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

			_transform.position = _startPosition;
			_transform.rotation = Quaternion.Euler(Vector3.zero);

			_currentDirection = Vector3.forward;
		}

		private void OnGameFailed()
		{
			_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}

		private void OnGameRestore()
		{
			_transform.position = _platformManager.CurrentPlatform._transform.position + _startPosition;
			_currentDirection = _platformManager.GetSafetyDirection();
		}
	}
}