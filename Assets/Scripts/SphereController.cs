using System;
using UnityEngine;

namespace ZigZag
{
    public class SphereController : MonoBehaviour
    {
        private const float _fallingHeight = -5f;

        private const float _speed = 5f;

        private bool isUp = true;

        private bool _isFailed = false;

        private Vector3 _currentDirection = Vector3.forward;

        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private Rigidbody _rigidbody;

        public event Action Falling;

        private void Update()
        {
            if (_isFailed == false)
            {
                if (_transform.position.y <= _fallingHeight)
                {
                    Destroy(_rigidbody);
                    //_rigidbody.useGravity = false;
                    _isFailed = true;

                    Falling?.Invoke();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isUp = !isUp;
                    _currentDirection = isUp ? Vector3.forward : Vector3.left;
                }
                Move();
            }
        }

        private void Move()
        {
            _transform.position += _speed * Time.deltaTime * _currentDirection;
        }

        public void ResetSphere()
        {
            _transform.position = new Vector3(0, 4f, 0);
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _isFailed = false;
        }
    }
}