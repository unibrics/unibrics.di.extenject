namespace Unibrics.Di.Extenject
{
    using System;
    using System.Collections.Generic;
    using Core;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        [SerializeField]
        private List<string> modulesToExclude;
        
        private Startup startup;
        
        public override void InstallBindings()
        {
            startup = new Startup(new ExtenjectService(Container), modulesToExclude);
            try
            {
                startup.Prepare();
                var sceneContext = GetComponentInParent<Zenject.SceneContext>();
                if (sceneContext)
                {
                    sceneContext.PostInstall += Launch;
                }
                else
                {
                    ProjectContext.PostInstall += Launch;
                }
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