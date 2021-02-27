using UnityEngine;
using Zenject;

namespace ZigZag
{
	public class GameCameraController : MonoBehaviour
	{
		private SphereController _sphere;

		private Vector3 _delta;

		[Inject]
		private void Construct(SphereController sphereController)
		{
			_sphere = sphereController;
			Vector3 delta = _sphere.transform.position - this.transform.position;
			_delta = new Vector3(Mathf.Abs(delta.x), Mathf.Abs(delta.y), -Mathf.Abs(delta.z));
		}

		private void FixedUpdate()
		{
			FollowToSphere();
		}

		private void FollowToSphere()
		{
			this.transform.position = _sphere.transform.position + _delta;
		}
	}
}