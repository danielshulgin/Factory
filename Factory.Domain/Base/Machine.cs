using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Machine : MachineBase
    {
        protected List<Detail> _detailsToAdd;
        
        public Machine(float _entityHandleTime, bool active, List<Detail> detailsToAdd) : base(_entityHandleTime, active)
        {
            if (detailsToAdd == null)
            {
                throw new NullReferenceException("details to add list = null");
            }
            _detailsToAdd = detailsToAdd;
        }

        public Machine(float _entityHandleTime, bool active) : this(_entityHandleTime, active, new List<Detail>()) { }

        public override void Accept(Entity entity)
        {
            if (entity == null)
            {
                throw new NullReferenceException("accepted entity = null");
            }
            _entitiesQueue.Enqueue(entity);
        }

        public override void Update(DateTime currentDateTime)
        {
            if (CanHandle())
            {
                EndHandleEntity();
                HandleEntity();
                _timeOfStartHandling = currentDateTime;
            }
        }

        public override bool CanHandle()
        {
            return ((DateTime.Now - _timeOfStartHandling).TotalSeconds > EntityHandleTime) ||
                _timeOfStartHandling == null;
        }

        public override void HandleEntity()
        {
            if (_entitiesQueue.Count > 0)
                _entitiesInProcess.Enqueue(_entitiesQueue.Dequeue());
        }

        public override void EndHandleEntity()
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

        public override Entity YieldEntity()
        {
            if (_entitiesComplete.Count > 0)
            {
                return _entitiesComplete.Dequeue();
            }
            return null;
        }
    }
}
