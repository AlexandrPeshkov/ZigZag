using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZigZag
{
    public class PlatformManager : MonoBehaviour
    {
        private const int _platformCapacity = 15;

        [SerializeField]
        private Transform _platformParent;

        [SerializeField]
        private Platform _platformPrefab;

        private LinkedList<Platform> _platforms;

        private float _platformSize;

        private Vector3 _top;

        private Vector3 _left;

        public event Action PlatfromComplete;

        private void Awake()
        {
            _platformSize = _platformPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * _platformPrefab.GetComponent<Transform>().localScale.x;

            _top = Vector3.forward * _platformSize;
            _left = Vector3.left * _platformSize;
            Init();
        }

        private void Init()
        {
            _platforms = new LinkedList<Platform>();
            FirstPlatform();

            for (var i = 1; i < _platformCapacity; i++)
            {
                GeneratePlatform(_platforms.Last.Value);
            }
        }

        private Platform FirstPlatform()
        {
            Platform platform = Instantiate(_platformPrefab, _platformParent, true);
            platform._transform.position = Vector3.zero;
            var node = _platforms.AddLast(platform);

            platform.LinkPlatform(node);
            return platform;
        }

        public Platform GeneratePlatform(Platform lastPlatform)
        {
            Platform platform = Instantiate(_platformPrefab, _platformParent, true);

            Vector3 platformShift = UnityEngine.Random.Range(0, 2) == 0 ? _top : _left;

            platform._transform.position = lastPlatform._transform.position + platformShift;

            platform.SpehreIsOut += OnSphereOut;

            var node = _platforms.AddLast(platform);
            platform.LinkPlatform(node);

            return platform;
        }

        /// <summary>
        /// Сфера покинула платформу
        /// </summary>
        /// <param name="lastPlatform"></param>
        private void OnSphereOut(Platform lastPlatform)
        {
            PlatfromComplete?.Invoke();

            Platform previousPlatform = lastPlatform.Node.Previous?.Previous?.Value;

            if (previousPlatform != null && previousPlatform != lastPlatform)
            {
                previousPlatform.SpehreIsOut -= OnSphereOut;

                _platforms.Remove(previousPlatform);
                previousPlatform.Fall();

                GeneratePlatform(_platforms.Last.Value);
            }
        }

        public void ResetPlatforms()
        {
            foreach (var platform in _platforms)
            {
                DestroyImmediate(platform.gameObject);
            }
            Init();
        }
    }
}