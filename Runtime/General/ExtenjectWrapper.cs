namespace Unibrics.Di.Extenject
{
    using System;
    using System.Linq;
    using Core.DI;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    public class ExtenjectWrapper : IInjector, IInstanceProvider, IResolver
    {
        private DiContainer container;

        public ExtenjectWrapper(DiContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// In case this wrapper is created via container, we're trying to replace default context with scene context.
        /// This will allow us to resolve dependencies in scene context too
        /// </summary>
        [Zenject.Inject]
        public void OnInjected(SceneContextRegistry registry)
        {
            if (registry.SceneContexts.Any())
            {
                foreach (var context in registry.SceneContexts)
                {
                    Debug.Log($"scene context: {context} {context.gameObject.name}");
                }
                container = registry.SceneContexts.Last().Container;
            }
        }

        public void InjectTo(object o)
        {
            if (o is GameObject go)
            {
                container.InjectGameObject(go);
            }
            else
            {
                container.Inject(o);
            }
        }

        public T GetInstance<T>()
        {
            try
            {
                return container.Instantiate<T>();
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                throw;
            }
        }

        public T GetInstance<T>(Type type)
        {
            try
            {
                return (T)container.Instantiate(type);
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                throw;
            }
        }

        public object GetInstance(Type type)
        {
            try
            {
                return container.Instantiate(type);
            }
            catch (Exception e)
            {
                //Debug.LogError(e);
                throw;
            }
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}