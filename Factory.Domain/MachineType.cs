using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class MachineType
    {
        private List<Detail> detail;
        private float EntityHandleTime;

        public MachineType(List<Detail> detail, float entityHandleTime)
        {
            this.detail = detail;
            EntityHandleTime = entityHandleTime;
        }
        //public float electricityConsumption;

    }
}
