using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class EntityOnTransposter
    {
        public float Position { get; private set; }
        public Entity Entity { get; private set; }

        private bool _done;

        public EntityOnTransposter(Entity entity, float postion = 0f)
        {
            this.Position = postion;
            this.Entity = entity;
            _done = false;
        }

        public void Update(Transporter transporter)
        {
            if(!_done)
                Position += transporter.PositionDelta;
        }

        public Entity EndTransporting()
        {
            _done = true;
            return this.Entity;
        }
    }
}
