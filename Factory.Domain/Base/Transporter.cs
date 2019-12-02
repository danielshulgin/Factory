using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Transporter :  MachineBase
    {
        public double Progress { get; private set; }
        public float PositionDelta { get; private set; }
        public float EntityTransportingTime { get; private set; }

        public IReadOnlyCollection<EntityOnTransposter> EntitiesOnTransporter => _entitiesOnTransporter;

        private Queue<EntityOnTransposter> _entitiesOnTransporter;

        private DateTime _lastUpdateTime;

        public Transporter(float entityTransportingTime, float _entityHandleTime, bool active) : base(_entityHandleTime, active)
        {
            EntityTransportingTime = entityTransportingTime;
            _entitiesOnTransporter = new Queue<EntityOnTransposter>();
        }

        public override void HandleEntity()
        {
            if (_entitiesQueue.Count > 0)
                _entitiesInProcess.Enqueue(_entitiesQueue.Dequeue());
        }

        public override void Update(DateTime currentDateTime)
        {
            float timeElapsed = (float)(DateTime.Now - _lastUpdateTime).TotalSeconds / EntityHandleTime;
            _lastUpdateTime = DateTime.Now;
            PositionDelta = timeElapsed / EntityTransportingTime;
            int readyEntities = 0;
            foreach (var entityOnTransposter in _entitiesOnTransporter)
            {
                entityOnTransposter.Update(this);
                if (entityOnTransposter.Position > 1f)
                {
                    readyEntities++;
                }
            }
            for (int i = 0; i < readyEntities; i++)
            {
                EndTransportingEntity();
            }
            if (CanHandle())
            {
                EndHandleEntity();
                HandleEntity();
                _timeOfStartHandling = currentDateTime;
            }
        }

        public override void EndHandleEntity()
        {
            if (_entitiesInProcess.Count > 0)
            {
                _entitiesOnTransporter.Enqueue(new EntityOnTransposter(
                    _entitiesInProcess.Dequeue()));
            }
        }

        protected void EndTransportingEntity()
        {
            var entity = _entitiesOnTransporter.Dequeue().EndTransporting();
            _entitiesComplete.Enqueue(entity);
        }

        public override void Accept(Entity entity)
        {
            _entitiesQueue.Enqueue(entity);
        }

        public override bool CanHandle()
        {
            return ((DateTime.Now - _timeOfStartHandling).TotalSeconds > EntityHandleTime) ||
                _timeOfStartHandling == null;
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
