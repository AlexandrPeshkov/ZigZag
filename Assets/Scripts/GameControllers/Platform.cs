using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZigZag
{
	public class Platform : MonoBehaviour
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

		public Transform _transform;

		public event Action<Platform> SpehreIsOut;

		public event Action<Platform> SphereIn;

		public LinkedListNode<Platform> Node { get; private set; }

		/// <summary>
		/// Очки ценности
		/// </summary>
		public int Points { get; private set; }

		public void InitPlatform(LinkedListNode<Platform> node, int cost)
		{
			Node = node;
			Points = cost;
		}

		public void Fall()
		{
			gameObject.AddComponent<Rigidbody>();
		}

		private void Update()
		{
			if (_transform.position.y <= _faidHeight)
			{
				DestroyImmediate(gameObject);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_visited == false)
			{
				HighLight();
				SphereIn?.Invoke(this);
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
	}
}