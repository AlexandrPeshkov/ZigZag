using UnityEngine;
using Zenject;

namespace ZigZag.Infrastructure
{
	[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
	public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
	{
		/// <summary>
		/// ������������ ZigZag
		/// </summary>
		[Header("��������� �������")]
		public BonusSettings _appConfig;

		public override void InstallBindings()
		{
			Container.BindInstance(_appConfig);
		}
	}
}