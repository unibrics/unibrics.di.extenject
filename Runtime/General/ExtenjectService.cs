namespace Unibrics.Di.Extenject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.DI;
    using Core.Services;
    using Tools;
    using UnityEngine;
    using Zenject;
    using Zenject.Internal;
    using InjectAttribute = Core.DI.InjectAttribute;

    class ExtenjectService : IDependencyInjectionService
    {
        private readonly ExtenjectWrapper wrapper;

        private readonly DiContainer diContainer;

        public IResolver Resolver => wrapper;

        private readonly List<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();

        public IInstanceProvider InstanceProvider => wrapper;
        
        public ExtenjectService(DiContainer diContainer)
        {
            this.diContainer = diContainer;
            wrapper = new ExtenjectWrapper(diContainer);
            ReflectionTypeAnalyzer.AddCustomInjectAttribute<InjectAttribute>();
        }
        
        public void Validate()
        {
            foreach (var descriptor in descriptors.Where(descriptor => !descriptor.IsConfirmed))
            {
                throw new UnibricsException(
                    $"Descriptor for types {string.Join(",", descriptor.InterfaceTypes.Select(type => type.Name))} " +
                    $"was not confirmed. Probably, you forget to set scope");
            }
        }

        public void PrepareService(ServiceDescriptor descriptor)
        {
            try
            {
                descriptor.Validate();
                var binding = diContainer.Bind(descriptor.InterfaceTypes);
                if (descriptor.ImplementationObject != null)
                {
                    binding.FromInstance(descriptor.ImplementationObject);
                    return;
                }

                var from = binding.To(descriptor.ImplementationType);
                if (descriptor.Scope == ServiceScope.Singleton)
                {
                    from.AsSingle();
                }
                else
                {
                    from.AsTransient();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Exception occured during Extenject DI service setup:", e);
            }
        }

        public void Add(ServiceDescriptor descriptor)
        {
            descriptors.Add(descriptor);
            descriptor.OnDescriptorFinalized(() => PrepareService(descriptor));
        }
    }
}