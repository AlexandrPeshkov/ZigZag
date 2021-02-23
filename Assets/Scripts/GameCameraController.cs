using UnityEngine;

namespace ZigZag
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField]
        private SphereController _sphere;

        private Vector3 _delta;

        private void Awake()
        {
            Vector3 delta = _sphere.transform.position - this.transform.position;
            _delta = new Vector3(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z));
        }

        private void Update()
        {
            FollowToSphere();
        }

        private void FollowToSphere()
        {
            //-x
            //+z
            this.transform.position = _sphere.transform.position + _delta;
        }
    }
}