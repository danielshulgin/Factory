using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
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
