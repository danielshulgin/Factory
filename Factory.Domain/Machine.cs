using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Machine
    {
        protected List<Machine> _input;
        protected List<Machine> _output;
        protected Queue<Entity> _entitiesInProcess;
        protected Queue<Entity> _entitiesQueue;
        protected List<Detail> _detailsToAdd;
        public float EntityHandleTime { get; private set; }

        //TODO get it out from bisnes logic, create seperate class for handling time
        protected DateTime TimeOfStartHandling;

        public bool Active { get; private set; }

        public int InProcess 
        {
            get
            {
                return _entitiesInProcess.Count;
            }
        }
        public int InQueue
        {
            get
            {
                return _entitiesQueue.Count;
            }
        }
        public Machine(float _entityHandleTime, bool active) : this(_entityHandleTime, active, new List<Detail>())
        {
            
        }

        public Machine(float _entityHandleTime, bool active, List<Detail> detailsToAdd)
        {
            TimeOfStartHandling = DateTime.Now;
            _input = new List<Machine>();
            _output = new List<Machine>();
            _entitiesInProcess = new Queue<Entity>();
            _entitiesQueue = new Queue<Entity>();
            _detailsToAdd = detailsToAdd;
            this.EntityHandleTime = _entityHandleTime;
            Active = active;
        }

        public virtual bool TryConnectInput(Machine machine)
        {
            _input.Add(machine);
            return true;
        }
        public virtual bool TryConnectOutput(Machine machine)
        {
            _output.Add(machine);
            return true;
        }

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
                TimeOfStartHandling = DateTime.Now;
            }
        }

        public virtual bool CanHandle()
        {
            return ((DateTime.Now - TimeOfStartHandling).TotalSeconds > EntityHandleTime) || 
                TimeOfStartHandling == null;
        }

        public virtual void EndHandleEntity()
        {
            if (_output != null && _output.Count > 0 && _entitiesInProcess.Count > 0)
            {
                Entity entity = _entitiesInProcess.Dequeue();
                foreach (var detail in _detailsToAdd)
                {
                    entity.TryAddDetail(detail);
                }
                _output[0].Accept(entity);
            }
        }

        public virtual void HandleEntity()
        {
            if(_entitiesQueue.Count > 0)
                _entitiesInProcess.Enqueue(_entitiesQueue.Dequeue());
        }

        public virtual void SetActive(bool active)
        {
            Active = active;
        }
    }
}
