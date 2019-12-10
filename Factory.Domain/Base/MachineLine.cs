using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class MachineLine
    {
        private List<MachineBase> _machines;

        public MachineLine(List<MachineBase> machines)
        {
            if (machines == null)
            {
                throw new ArgumentNullException("machines");
            }
            if (machines.Count > 0)
            {
                CheckMachineOrder(machines);
            }
            this._machines = machines;
        }

        private void CheckMachineOrder(List<MachineBase> machines)
        {
            if (!(machines[0] is MachineSource))
            {
                throw new Exception("machine 0 must be of type MachineSource");
            }
            for (int i = 1; i < machines.Count; i++)
            {
                if (machines[i] is MachineSource)
                {
                    throw new Exception($"machine {i} of type MachineSource");
                }
            }
            if (machines.Count % 2 == 0)
            {
                throw new Exception($"illegal number of machines({machines.Count})");
            }
            for (int i = 1; i < machines.Count; i += 2)
            {
                if (!(machines[i] is Transporter))
                {
                    throw new Exception($"machine {i} must be of type Transporter");
                }
                if ((machines[i + 1] is Transporter))
                {
                    throw new Exception($"machine {i + 1} of type Transporter");
                }
            }
        }

        public void Update(DateTime currentDateTime)
        {
            if (_machines.Count >= 2)
            {
                for (int i = 0; i < _machines.Count - 1; i++)
                {
                    MachineBase nextMachine = _machines[i + 1];
                    UpdateMachine(_machines[i], nextMachine, currentDateTime);
                }
            }
        }

        private void UpdateMachine(MachineBase machine, MachineBase nextMachine, DateTime currentDateTime)
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
