namespace Unibrics.Di.Extenject
{
    using Core.DI;
    using Core.Launchers;

    public class ExtenjectLauncher : IModuleLauncher
    {
        [Inject]
        public IInstanceProvider InstanceProvider { get; set; }
        
        public void Launch()
        {
            InstanceProvider.GetInstance<TickableProxy>();
        }
    }
}