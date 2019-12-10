using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Transporter :  MachineBase
    {

        public double X1 { get; protected set; }

        public double Y1 { get; protected set; }

        public double X2 { get; protected set; }

        public double Y2 { get; protected set; }

        public double Progress { get; private set; }

        public double PositionDelta { get; private set; }

        public double EntityTransportingTime { get; private set; }

        public IReadOnlyCollection<EntityOnTransposter> EntitiesOnTransporter => _entitiesOnTransporter;
        [JsonProperty]
        private Queue<EntityOnTransposter> _entitiesOnTransporter;
        [JsonProperty]
        private DateTime _lastUpdateTime;

        public Transporter(double entityTransportingTime = 0d, double entityHandleTime = 0d, bool active = true,
            double x = 0d, double y = 0d, double x1 = 0d, double y1 = 0d, double x2 = 0d, double y2 = 0d) : base(entityHandleTime, active, x, y)
        {
            EntityTransportingTime = entityTransportingTime;
            _entitiesOnTransporter = new Queue<EntityOnTransposter>();
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override void HandleEntity()
        {
            if (_entitiesQueue.Count > 0)
                _entitiesInProcess.Enqueue(_entitiesQueue.Dequeue());
        }

        public override void Update(DateTime currentDateTime)
        {
            double timeElapsed = (DateTime.Now - _lastUpdateTime).TotalSeconds / EntityHandleTime;
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

        public void Reset()
        {
            _entitiesOnTransporter = new Queue<EntityOnTransposter>();
        }
    }
}
