using UnityEngine;
using Zenject;

namespace ZigZag.Infrastructure
{
	[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
	public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
	{
		/// <summary>
		/// Конфигурация ZigZag
		/// </summary>

		public GameConfig _appConfig;

		public override void InstallBindings()
		{
			Container.BindInstance(_appConfig);
		}
	}
}