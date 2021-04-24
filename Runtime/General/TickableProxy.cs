namespace Unibrics.Di.Extenject
{
    using System;
    using System.Collections.Generic;
    using Core.DI;

    public class TickableProxy : Zenject.ITickable, ITickProvider
    {
        private readonly ITickable[] tickables;

        private readonly List<CancelableTickAction> tickActions = new List<CancelableTickAction>();

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

            for (int i = tickActions.Count - 1; i >= 0; i--)
            {
                var action = tickActions[i];
                if (action.IsCanceled)
                {
                    tickActions.RemoveAt(i);
                }
                else
                {
                    action.Execute();
                }
            }
        }

        public ICancelable OnTick(Action action)
        {
            if (action == null)
            {
                throw new ArgumentException("can not schedule null action for every tick");
            }

            var cancelableTickAction = new CancelableTickAction(action);
            tickActions.Add(cancelableTickAction);
            return cancelableTickAction;
        }

        class CancelableTickAction : ICancelable
        {
            private readonly Action action;
            public bool IsCanceled { get; private set; }

            public CancelableTickAction(Action action)
            {
                this.action = action;
            }

            public void Execute()
            {
                action();
            }

            public void Cancel()
            {
                IsCanceled = true;
            }
        }
    }

}