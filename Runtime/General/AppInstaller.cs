namespace Unibrics.Di.Extenject
{
    using System;
    using Core;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var startup = new Startup(new ExtenjectService(Container));
            try
            {
                startup.StartSequence();
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