namespace Unibrics.Di.Extenject
{
    using System;
    using Core.DI;
    using UnityEngine;
    using Zenject;

    public class ExtenjectWrapper : IInjector, IInstanceProvider, IResolver
    {
        private readonly DiContainer container;

        public ExtenjectWrapper(DiContainer container)
        {
            this.container = container;
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
                Debug.LogError(e);
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
                Debug.LogError(e);
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
                Debug.LogError(e);
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