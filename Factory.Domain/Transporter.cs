using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Transporter : Machine
    {
        public double Progress { get; private set; }
        public float EntityTransportingTime { get; private set; }
        public Queue<EntityOnTransposter> EntitiesOnTransporter { get; private set; }
        private DateTime _lastUpdateTime;
        //TODO in main update loop

        public Transporter(float entityTransportingTime, float _entityHandleTime, bool active) : base(_entityHandleTime, active)
        {
            EntityTransportingTime = entityTransportingTime;
            EntitiesOnTransporter = new Queue<EntityOnTransposter>();
        }

        public override void HandleEntity()
        {
            base.HandleEntity();
        }

        public override void Update()
        {
            float timeElapsed = (float)(DateTime.Now - _lastUpdateTime).TotalSeconds / EntityHandleTime;
            _lastUpdateTime = DateTime.Now;
            float positionDelta = timeElapsed / EntityTransportingTime;
            int readyEntities = 0;
            foreach (var entityOnTransposter in EntitiesOnTransporter)
            {
                entityOnTransposter.postion += positionDelta;
                if (entityOnTransposter.postion > 1f)
                {
                    readyEntities++;
                }
            }
            for (int i = 0; i < readyEntities; i++)
            {
                EndTransportingEntity();
            }
            base.Update();
        }

        public override void EndHandleEntity()
        {
            if (_output != null && _output.Count > 0 && _entitiesInProcess.Count > 0)
            {
                EntitiesOnTransporter.Enqueue(new EntityOnTransposter(
                    _entitiesInProcess.Dequeue()));
            }
        }

        public virtual void EndTransportingEntity()
        {
            var entity = EntitiesOnTransporter.Dequeue().entity;
            _output[0].Accept(entity);
        }
    }
}
