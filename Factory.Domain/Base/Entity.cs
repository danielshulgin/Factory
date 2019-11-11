using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Factory.Domain
{
    public class Entity
    {
        public List<string> DetailsNames => _details.Select(detail => detail.Name).ToList();

        private List<Detail> _details;
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
