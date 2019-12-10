using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class MachineSource : Machine
    {
        public MachineSource(double entityHandleTime = 0d, bool active = true, List<Detail> detailsToAdd = null,
            double x = 0d, double y = 0d) : base(entityHandleTime, active, detailsToAdd, x, y)
        {
        }

        public override void EndHandleEntity()
        {
            Entity entity = new Entity();
            foreach (var detail in _detailsToAdd)
            {
                entity.TryAddDetail(detail);
            }
            _entitiesComplete.Enqueue(entity);
        }
    }
}
