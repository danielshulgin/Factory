using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Transporter : Machine
    {
        public double progress = 0;
        public Queue<EntityOnTransposter> _entitiesOnTransporter = new Queue<EntityOnTransposter>();
        private DateTime _lastUpdateTime;
        public float EntityTransportingTime { get; private set; }
        //TODO in main update loop

        public Transporter(float entityTransportingTime, float _entityHandleTime, bool active) : base(_entityHandleTime, active)
        {
            EntityTransportingTime = entityTransportingTime;
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
            foreach (var entityOnTransposter in _entitiesOnTransporter)
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
                _entitiesOnTransporter.Enqueue(new EntityOnTransposter(
                    _entitiesInProcess.Dequeue()));
            }
        }

        public virtual void EndTransportingEntity()
        {
            var entity = _entitiesOnTransporter.Dequeue().entity;
            _output[0].Accept(entity);
        }
    }

    public class EntityOnTransposter
    {
        public float postion;
        public Entity entity;

        public EntityOnTransposter(Entity entity, float postion = 0f)
        {
            this.postion = postion;
            this.entity = entity;
        }
    }
}

