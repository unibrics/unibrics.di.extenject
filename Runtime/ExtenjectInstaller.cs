namespace Unibrics.Di.Extenject
{
    using Core;
    using Core.DI;
    using Core.Services;
    using ITickable = Zenject.ITickable;

    [Install]
    public class ExtenjectInstaller : ModuleInstaller<ExtenjectLauncher>
    {
        public override Priority Priority => Priority.High;

        public override void Install(IServicesRegistry services)
        {
            services.Add(typeof(ITickable), typeof(ITickProvider))
                .ImplementedBy<TickableProxy>().AsSingleton();
            services.Add(typeof(IInjector), typeof(IInstanceProvider), typeof(IResolver)).ImplementedBy<ExtenjectWrapper>().AsSingleton();
        }
    }
}