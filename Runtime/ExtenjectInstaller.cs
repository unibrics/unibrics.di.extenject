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
            services.AddSingleton<ExtenjectWrapper>(typeof(IInjector), typeof(IInstanceProvider), typeof(IResolver));
            services.AddSingleton<ITickable, TickableProxy>();
        }
    }
}