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
		[Header("Настройки бонусов")]
		public BonusSettings _appConfig;

		public override void InstallBindings()
		{
			Container.BindInstance(_appConfig);
		}
	}
}