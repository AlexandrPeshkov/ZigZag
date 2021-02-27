using UnityEngine;
using Zenject;
using ZigZag.Abstracts;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag
{
	public class SphereController : MonoBehaviour
	{
		private const float _fallingHeight = -5f;

		private const float _velocity = 5f;

		private const float _angularVelocity = 10;

		private Vector3 _currentDirection = Vector3.forward;

		private Vector3 _startPosition;

		[SerializeField]
		private Transform _transform;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Platform _platformPrefab;

		private GameStateService _gameStateService;

		private InputHandler _inputHandler;

		[Inject]
		private void Construct(GameStateService gameState, InputHandler inputHandler)
		{
			_gameStateService = gameState;
			_inputHandler = inputHandler;

			gameState.GameStateChanged += OnGameStateChanged;

			_inputHandler.LeftMouseButtonUp += OnLeftMouseButtonUp;

			var platformHeight = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * _platformPrefab.GetComponent<Transform>().localScale.y * 0.5f;
			var sphereRadius = this.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * this.GetComponent<Transform>().localScale.y * 0.5f;
			var y = platformHeight + sphereRadius;
			_startPosition = new Vector3(0, platformHeight + sphereRadius, 0);
		}

		private void OnLeftMouseButtonUp()
		{
			if (_gameStateService.State == GameState.Run)
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
						OnGameReset();
						break;
					}
				case GameState.Failed:
					{
						OnGameFailed();
						break;
					}
			}
		}

		private void FixedUpdate()
		{
			if (_gameStateService.State == GameState.Run)
			{
				if (_transform.position.y <= _fallingHeight)
				{
					_gameStateService.ChangeState(GameState.Failed);
				}
				else
				{
					Move();
					//Rotate();
				}
			}
		}

		private void Move()
		{
			_transform.position += _velocity * Time.deltaTime * _currentDirection;
		}

		//TODO Разобраться с вращением сферы
		private void Rotate()
		{
			if (_currentDirection == Vector3.forward)
			{
				//_transform.rotation = Quaternion.Euler(_transform.rotation.eulerAngles + _angleSpeed * Time.deltaTime * Vector3.right);
				_transform.Rotate(_angularVelocity * Time.deltaTime * Vector3.right);
			}
			else
			{
				//_transform.localEulerAngles += _angleSpeed * Time.deltaTime * Vector3.forward;
				transform.Rotate(_angularVelocity * Time.deltaTime * Vector3.back);
			}
		}

		private void OnGameReset()
		{
			_rigidbody.constraints = RigidbodyConstraints.None;
			_transform.position = _startPosition;
			_transform.rotation = Quaternion.Euler(Vector3.zero);

			_currentDirection = Vector3.forward;
		}

		private void OnGameFailed()
		{
			_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}
}