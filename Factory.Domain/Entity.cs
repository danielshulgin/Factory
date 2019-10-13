using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class Entity
    {
        public readonly List<Detail> _details;

        public Entity()
        {
            _details = new List<Detail>();
        }

        public bool TryAddDetail(Detail detail)
        {
            _details.Add(detail);
            return true;
        }
    }
}
