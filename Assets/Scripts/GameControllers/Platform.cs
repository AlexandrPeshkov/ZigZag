using System;
using UnityEngine;
using Zenject;
using ZigZag.Infrastructure;
using ZigZag.Services;

namespace ZigZag
{
	[RequireComponent(typeof(MeshFilter))]
	public class Platform : MonoBehaviour, IPoolable<int, Vector3, IMemoryPool>, IDisposable
	{
		private bool _visited = false;

		[SerializeField]
		private Material _avtiveMaterial;

		[SerializeField]
		private Material _normalMaterial;

		[SerializeField]
		private MeshRenderer _meshRenderer;

		private ScoreService _scoreService;

		private GameStateService _gameState;

		private IMemoryPool _memoryPool;

		public Transform _transform;

		public event Action<Platform> SphereOut;

		public event Action<Platform> SphereIn;

		public event Action<Platform> Disposed;

		public Rect TopRect { get; private set; }

		/// <summary>
		/// Очки ценности
		/// </summary>
		public int Points { get; private set; }

		[Inject]
		private void Construct(ScoreService scoreService, GameStateService gameStateService)
		{
			_scoreService = scoreService;
			_gameState = gameStateService;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name == SphereController._objectName && _visited == false)
			{
				_visited = true;
				HighLight();
				_scoreService.AddPoints(Points);
				SphereIn?.Invoke(this);
			}

			if (_gameState.State == Abstracts.GameState.Failing)
			{
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.name == SphereController._objectName && _visited == true)
			{
				SphereOut?.Invoke(this);
			}
			UnHighLight();
		}

		private void UnHighLight()
		{
			_meshRenderer.material = _normalMaterial;
		}

		private void HighLight()
		{
			_meshRenderer.material = _avtiveMaterial;
		}

		private void UpdateTopRect()
		{
			var platformSize = GetComponent<MeshFilter>().sharedMesh.bounds.size;
			var platformWidth = platformSize.x * _transform.localScale.x;
			var platformLength = platformSize.z * _transform.localScale.z;

			Vector2 bottomLeft = new Vector2(transform.position.x - platformWidth * 0.5f, transform.position.z - platformLength * 0.5f);

			TopRect = new Rect(bottomLeft.x, bottomLeft.y, platformWidth, platformLength);
		}

		#region IPoolable

		public void OnSpawned(int points, Vector3 pos, IMemoryPool pool)
		{
			_memoryPool = pool;
			Points = points;
			_transform.position = pos;
			UpdateTopRect();
		}

		public void OnDespawned()
		{
			_memoryPool = null;
			Points = 0;
			_visited = false;
			TopRect = new Rect();
			UnHighLight();
		}

		public void Dispose()
		{
			_memoryPool.Despawn(this);
			Disposed?.Invoke(this);
		}

		#endregion IPoolable

		public class Factory : PlaceholderFactory<int, Vector3, Platform>
		{
		}
	}
}