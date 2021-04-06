namespace Unibrics.Di.Extenject
{
    using Core;
    using Core.DI;
    using Core.Services;

    [Install]
    public class ExtenjectInstaller : ModuleInstaller
    {
        public override Priority Priority => Priority.High;
        
        public override void Install(IServicesRegistry services)
        {
            services.AddSingleton<ExtenjectWrapper>(typeof(IInjector), typeof(IInstanceProvider), typeof(IResolver));
        }
    }
}