using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Machine
    {
        public float EntityHandleTime { get; private set; }
        public bool Active { get; private set; }
        
        protected Queue<Entity> _entitiesQueue;
        protected Queue<Entity> _entitiesInProcess;
        protected Queue<Entity> _entitiesComplete;
        public int InProcessEntityNumber => _entitiesInProcess.Count;
        public int InQueueEntityNumber => _entitiesQueue.Count;
        public int CompleteEntityNumber => _entitiesComplete.Count;
        
        protected List<Detail> _detailsToAdd;
        protected DateTime _timeOfStartHandling;
       
        public Machine(float _entityHandleTime, bool active, List<Detail> detailsToAdd)
        {
            _timeOfStartHandling = DateTime.Now;
            
            _entitiesInProcess = new Queue<Entity>();
            _entitiesQueue = new Queue<Entity>();
            _entitiesComplete = new Queue<Entity>();

            _detailsToAdd = detailsToAdd;
            this.EntityHandleTime = _entityHandleTime;
            Active = active;
        }

        public Machine(float _entityHandleTime, bool active) : this(_entityHandleTime, active, new List<Detail>()) { }

        public virtual void Accept(Entity entity)
        {
            _entitiesQueue.Enqueue(entity);
        }

        public virtual void Update()
        {
            if (CanHandle())
            {
                EndHandleEntity();
                HandleEntity();
                _timeOfStartHandling = DateTime.Now;
            }
        }

        public virtual bool CanHandle()
        {
            return ((DateTime.Now - _timeOfStartHandling).TotalSeconds > EntityHandleTime) || 
                _timeOfStartHandling == null;
        }

        public virtual void HandleEntity()
        {
            if(_entitiesQueue.Count > 0)
                _entitiesInProcess.Enqueue(_entitiesQueue.Dequeue());
        }

        public virtual void EndHandleEntity()
        {
            if (_entitiesInProcess.Count > 0)
            {
                Entity entity = _entitiesInProcess.Dequeue();
                foreach (var detail in _detailsToAdd)
                {
                    entity.TryAddDetail(detail);
                }
                _entitiesComplete.Enqueue(entity);
            }
        }

        public Entity YieldEntity()
        {
            if (_entitiesComplete.Count > 0)
            {
                return _entitiesComplete.Dequeue();
            }
            return null;
        }

        

        public virtual void SetActive(bool active)
        {
            Active = active;
        }
    }
}
