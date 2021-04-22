namespace Unibrics.Di.Extenject
{
    using System;
    using Core;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        private Startup startup;
        
        public override void InstallBindings()
        {
            startup = new Startup(new ExtenjectService(Container));
            try
            {
                startup.Prepare();
                ProjectContext.PostInstall += Launch;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured during App Setup:");
                Debug.LogError(e);
                throw;
            }
        }

        private void Launch()
        {
            try
            {
                startup.Start();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured during App Launch:");
                Debug.LogError(e);
                throw;
            }
        }
    }
}