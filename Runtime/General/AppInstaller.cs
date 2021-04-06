namespace Unibrics.Di.Extenject
{
    using Core;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var startup = new Startup(new ExtenjectService(Container));
            startup.StartApp();
        }
    }
}