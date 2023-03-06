namespace Unibrics.Di.Extenject.SceneContext
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.DI.SceneContext;
    using Tools;
    using UnityEngine.SceneManagement;
    using Zenject;

    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var projectContextContainer = Container.ParentContainers[0];
            var diService = new ExtenjectService(Container);

            foreach (var installer in GetInstallers(projectContextContainer))
            {
                installer.Install(diService);
            }
            
            diService.PrepareServices();
        }

        private static IEnumerable<SceneContextInstaller> GetInstallers(IInstantiator container) => Types.AnnotatedWith<InstallAttribute>()
            .WithParent(typeof(SceneContextInstaller))
            .TypesOnly()
            .Select(type => (SceneContextInstaller)container.Instantiate(type))
            .Where(installer => installer.SceneName.Equals(SceneManager.GetActiveScene().name));
    }
}