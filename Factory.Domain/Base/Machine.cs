using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Machine : MachineBase
    {
        [JsonProperty]
        protected List<Detail> _detailsToAdd;
        
        public Machine(double entityHandleTime = 0d, bool active = true, List<Detail> detailsToAdd = null, 
            double x = 0d, double y = 0d) : base(entityHandleTime, active, x, y)
        {
            if (detailsToAdd == null)
            {
                //throw new ArgumentNullException("details to add");
            }
            _detailsToAdd = detailsToAdd;
        }

        public override void Accept(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("accepted entity");
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
