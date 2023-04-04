namespace Unibrics.Di.Extenject.SceneContext
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.DI.SceneContext;
    using Tools;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;
    using Types = Tools.Types;

    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private string sceneName;
        
        public override void InstallBindings()
        {
            var parentContainer = Container;
            var diService = new ExtenjectService(Container);

            foreach (var installer in GetInstallers(parentContainer))
            {
                installer.Install(diService);
            }
            
            diService.PrepareServices();
        }

        private IEnumerable<SceneContextInstaller> GetInstallers(IInstantiator container) => Types.AnnotatedWith<InstallAttribute>()
            .WithParent(typeof(SceneContextInstaller))
            .TypesOnly()
            .Select(type => (SceneContextInstaller)container.Instantiate(type))
            .Where(installer => installer.SceneName.Equals(sceneName));
    }
}