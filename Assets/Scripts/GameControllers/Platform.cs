using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZigZag
{
    public class Platform : MonoBehaviour
    {
        private const float _minHeight = -30;

        private bool _visited = false;

        [SerializeField]
        private Material _avtiveMaterial;

        [SerializeField]
        private Material _normalMaterial;

        [SerializeField]
        private MeshRenderer _meshRenderer;

        public Transform _transform;

        public event Action<Platform> SpehreIsOut;

        public LinkedListNode<Platform> Node { get; private set; }

        public void LinkPlatform(LinkedListNode<Platform> node)
        {
            Node = node;
        }

        public void Fall()
        {
            gameObject.AddComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_transform.position.y <= _minHeight)
            {
                DestroyImmediate(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_visited == false)
            {
                HighLight();
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