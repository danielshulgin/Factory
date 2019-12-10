using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Factory.Domain
{
    public abstract class MachineBase
    {
        public bool Active { get; protected set; }

        public double EntityHandleTime { get; protected set; }

        public double X { get; protected set; }

        public double Y { get; protected set; }

        public virtual int InProcessEntityNumber => _entitiesInProcess.Count;
        
        public virtual int InQueueEntityNumber => _entitiesQueue.Count;
        
        public virtual int CompleteEntityNumber => _entitiesComplete.Count;

        protected Queue<Entity> _entitiesQueue;

        protected Queue<Entity> _entitiesInProcess;

        protected Queue<Entity> _entitiesComplete;

        protected DateTime _timeOfStartHandling;

        public MachineBase(double entityHandleTime = 0d, bool active = true, double x = 0d, double y = 0d)
        {
            if (entityHandleTime < 0 )
            {
                throw new Exception("Entity handle time must be greater than zero!");
            }
            _timeOfStartHandling = DateTime.Now;

            _entitiesInProcess = new Queue<Entity>();
            _entitiesQueue = new Queue<Entity>();
            _entitiesComplete = new Queue<Entity>();

            EntityHandleTime = entityHandleTime;
            Active = active;
            X = x;
            Y = y;
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

        public virtual void SetPosition(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}