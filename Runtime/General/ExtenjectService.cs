namespace Unibrics.Di.Extenject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.DI;
    using Core.Services;
    using Zenject;
    using Zenject.Internal;
    using InjectAttribute = Core.DI.InjectAttribute;

    class ExtenjectService : IDependencyInjectionService
    {
        public IInstanceProvider InstanceProvider => new ExtenjectWrapper(diContainer);
        
        private readonly DiContainer diContainer;

        private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();

        public ExtenjectService(DiContainer diContainer)
        {
            this.diContainer = diContainer;
            ReflectionTypeAnalyzer.AddCustomInjectAttribute<InjectAttribute>();
        }
        
        public void PrepareServices()
        {
            try
            {
                foreach (var descriptor in descriptors)
                {
                    descriptor.Validate();
                    var binding = diContainer.Bind(descriptor.InterfaceTypes);
                    if (descriptor.ImplementationObject != null)
                    {
                        binding.FromInstance(descriptor.ImplementationObject);
                        if (descriptor.QueueForInjection)
                        {
                            diContainer.QueueForInject(descriptor.ImplementationObject);
                        }
                        continue;
                    }

                    var from = binding.To(descriptor.ImplementationType);
                    if (descriptor.Scope == ServiceScope.Singleton)
                    {
                        var bind = from.AsSingle();
                        if (descriptor.InterfaceTypes.All(type => !type.IsGenericTypeDefinition))
                        {
                            bind.NonLazy();
                        }
                    }
                    else
                    {
                        from.AsTransient();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Exception occured during Extenject DI setup:", e);
            }
        }

        public void Add(ServiceDescriptor descriptor)
        {
            descriptors.Add(descriptor);
        }

        ServiceDescriptor IServicesRegistry.Get(Predicate<ServiceDescriptor> filter)
        {
            return descriptors.FirstOrDefault(descriptor => filter(descriptor));
        }

        void IServicesRegistry.Drop(ServiceDescriptor descriptor)
        {
            descriptors.Remove(descriptor);
        }
    }
}