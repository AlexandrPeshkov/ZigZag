using System;
using UnityEngine;

namespace ZigZag.Infrastructure
{
	/// <summary>
	/// Ресивер пользовательского ввода
	/// </summary>
	public class InputHandler : MonoBehaviour
	{
		public event Action LeftMouseButtonUp;

		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				LeftMouseButtonUp?.Invoke();
			}
		}
	}
}