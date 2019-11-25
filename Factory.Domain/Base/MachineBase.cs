using System;
using System.Collections.Generic;

namespace Factory.Domain
{
    public abstract class MachineBase
    {
        public bool Active { get; protected set; }

        public float EntityHandleTime { get; protected set; }

        public virtual int InProcessEntityNumber => _entitiesInProcess.Count;
        
        public virtual int InQueueEntityNumber => _entitiesQueue.Count;
        
        public virtual int CompleteEntityNumber => _entitiesComplete.Count;

        protected Queue<Entity> _entitiesQueue;

        protected Queue<Entity> _entitiesInProcess;

        protected Queue<Entity> _entitiesComplete;

        protected DateTime _timeOfStartHandling;

        public MachineBase(float _entityHandleTime, bool active)
        {
            _timeOfStartHandling = DateTime.Now;

            _entitiesInProcess = new Queue<Entity>();
            _entitiesQueue = new Queue<Entity>();
            _entitiesComplete = new Queue<Entity>();

            this.EntityHandleTime = _entityHandleTime;
            Active = active;
        }

        public abstract void Accept(Entity entity);

        public abstract bool CanHandle();

        public abstract void EndHandleEntity();

        public abstract void HandleEntity();

        public abstract void Update(DateTime currentDateTime);

        public abstract Entity YieldEntity();

        public virtual void SetActive(bool active)
        {
            Active = active;
        }
    }
}