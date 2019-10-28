using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class MachineLine
    {
        private List<Machine> _machines;

        public MachineLine(List<Machine> machines)
        {
            this._machines = machines;
        }

        public void Update()
        {
            if (_machines.Count >= 2)
            {
                for (int i = 0; i < _machines.Count - 1; i++)
                {
                    UpdateMachine(_machines[i], _machines[i + 1]);
                }
            }
        }

        public void UpdateMachine(Machine machine, Machine nextMachine)
        {
            machine.Update();
            if (machine.CompleteEntityNumber > 0)
            {
                Entity entity = machine.YieldEntity();
                nextMachine.Accept(entity);
            }
        }

        public void Optimaize()
        {

        }
    }
}
