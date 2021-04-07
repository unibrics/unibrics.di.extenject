namespace Unibrics.Di.Extenject
{
    using System.Collections.Generic;
    using Core.DI;

    public class TickableProxy : Zenject.ITickable
    {
        private readonly ITickable[] tickables;

        [Inject]
        public TickableProxy(ITickable[] tickables)
        {
            this.tickables = tickables;
        }

        public void Tick()
        {
            for (var i = 0; i < tickables.Length; i++)
            {
                tickables[i].Tick();
            }
        }
    }
}