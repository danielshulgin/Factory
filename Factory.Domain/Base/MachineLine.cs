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

        public void Update(DateTime currentDateTime)
        {
            if (_machines.Count >= 2)
            {
                for (int i = 0; i < _machines.Count - 1; i++)
                {
                    Machine nextMachine = _machines[i + 1];
                    UpdateMachine(_machines[i], nextMachine, currentDateTime);
                }
            }
        }

        private void UpdateMachine(Machine machine, Machine nextMachine, DateTime currentDateTime)
        {
            machine.Update(currentDateTime);
            if (machine.CompleteEntityNumber > 0)
            {
                Entity entity = machine.YieldEntity();
                nextMachine.Accept(entity);
            }
        }
    }
}
