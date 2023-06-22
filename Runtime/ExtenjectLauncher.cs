namespace Unibrics.Di.Extenject
{
    using System.Collections.Generic;
    using Core.DI;
    using Core.Launchers;

    public class ExtenjectLauncher : ModuleLauncher
    {
        [Inject]
        public IInstanceProvider InstanceProvider { get; set; }

        public override void Launch()
        {
            InstanceProvider.GetInstance<TickableProxy>();
        }
    }
}