using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ZigZag
{
	public class Platform : MonoBehaviour, IPoolable<int, IMemoryPool>, IDisposable
	{
		/// <summary>
		/// Высота исчезания
		/// </summary>
		private const float _faidHeight = -30;

		private bool _visited = false;

		[SerializeField]
		private Material _avtiveMaterial;

		[SerializeField]
		private Material _normalMaterial;

		[SerializeField]
		private MeshRenderer _meshRenderer;

		private ScoreService _scoreService;

		private IMemoryPool _memoryPool;

		public Transform _transform;

		public event Action<Platform> SpehreIsOut;

		/// <summary>
		/// Очки ценности
		/// </summary>
		public int Points { get; private set; }

		[Inject]
		private void Construct(ScoreService scoreService)
		{
			_scoreService = scoreService;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_visited == false)
			{
				HighLight();
				_scoreService.AddPoints(Points);
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if (_visited == false)
			{
				SpehreIsOut?.Invoke(this);
			}
			_visited = true;
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

		#region IPoolable

		public void OnSpawned(int points, IMemoryPool pool)
		{
			_memoryPool = pool;
			Points = points;
		}

		public void OnDespawned()
		{
			_memoryPool = null;
			Points = 0;
			_visited = false;
		}

		public void Dispose()
		{
			_memoryPool.Despawn(this);
		}

		#endregion IPoolable

		public class Factory : PlaceholderFactory<int, Platform> { }
	}
}