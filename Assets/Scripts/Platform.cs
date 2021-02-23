using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag
{
    public class Platform : MonoBehaviour
    {
        private bool _visited = false;

        private float _minHeight = -30;

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

        private void OnCollisionExit(Collision collision)
        {
            if (_visited == false)
            {
                SpehreIsOut?.Invoke(this);
            }
            _visited = true;
        }
    }
}