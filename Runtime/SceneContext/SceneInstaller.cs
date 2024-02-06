namespace Unibrics.Di.Extenject.SceneContext
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.DI;
    using Core.DI.SceneContext;
    using Core.Execution;
    using Core.Services;
    using Tools;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;
    using Types = Tools.Types;
    

    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private string sceneName;

        private IDependencyInjectionService diService;

        public override void InstallBindings()
        {
            var par = Container;
            diService = new ExtenjectService(Container);

            foreach (var installer in GetInstallers(par))
            {
                installer.Install(diService);
            }

            diService.Add(typeof(IResolver), typeof(IInstanceProvider), typeof(IInjector)).ImplementedByInstance(new ExtenjectWrapper(Container));
            diService.InstallCoreComponents();
            diService.PrepareServices();
        }

        public override void Start()
        {
            diService.Resolver.Resolve<IInitializablesRegistry>().StartNewInitializables(diService);
        }

        private IEnumerable<SceneContextInstaller> GetInstallers(IInstantiator container) => Types
            .AnnotatedWith<InstallAttribute>()
            .WithParent(typeof(SceneContextInstaller))
            .TypesOnly()
            .Select(type => (SceneContextInstaller)container.Instantiate(type))
            .OrderByDescending(installer => installer.Priority)
            .Where(installer => installer.SceneName.Equals(sceneName));
    }
}