namespace Unibrics.Di.Extenject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.DI.Environment;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        [SerializeField, Tooltip("Specify module tags to NOT install. Option has lowest priority")]
        private List<string> tagsToExclude;
        
        [SerializeField, Tooltip("Specify module ids to NOT install")]
        private List<string> modulesToExclude;

        [SerializeField, Tooltip("Specify module ids to install. Option has highest priority and will overwrite tag")]
        private List<string> modulesToInclude;
        
        private Startup startup;

        private Zenject.SceneContext sceneContext;
        
        public override void InstallBindings()
        {
            startup = new Startup(GetEnvironment(), new ExtenjectService(Container), ShouldInclude);
            try
            {
                startup.Prepare();
                sceneContext = GetComponentInParent<Zenject.SceneContext>();
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

        private IEnvironment GetEnvironment()
        {
            var provider = GetComponent<EnvironmentProvider>();
            if (provider)
            {
                return provider.GetEnvironment();
            }

            return new DefaultEnvironment();
        }

        private bool ShouldInclude(ModuleDescriptor descriptor)
        {
            if (descriptor.Id != null && modulesToInclude.Contains(descriptor.Id))
            {
                return true;
            }

            if (descriptor.Id != null && modulesToExclude.Contains(descriptor.Id))
            {
                return false;
            }

            return tagsToExclude.All(tag => !descriptor.Tags.Contains(tag));
        }

        private void OnDestroy()
        {
            if (sceneContext)
            {
                sceneContext.PostInstall -= Launch;
            }
            else
            {
                ProjectContext.PostInstall -= Launch;
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