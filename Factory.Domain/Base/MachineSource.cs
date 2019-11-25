using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class MachineSource : Machine
    {
        public MachineSource(float _entityHandleTime, bool active, List<Detail> detailsToAdd) : base(_entityHandleTime, active, detailsToAdd)
        {
        }

        public override void Update(DateTime currentDateTime)
        {
            base.Update(currentDateTime);
            _entitiesComplete.Enqueue(new Entity());
        }
    }
}
