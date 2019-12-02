using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public struct Detail : IEquatable<Detail>
    {
        public string Name { get; private set; }

        public int Cost { get; private set; }
        public Guid Guid { get; private set; }

        public Detail(string name, Guid guid, int cost = 0)
        {
            if (cost < 0 )
            {
                throw new Exception("Cost must be greater than zero!");
            }
            if (guid == Guid.Empty)
            {
                throw new Exception("Empty guid");
            }
            Name = name;
            Cost = cost;
            this.Guid = guid;
        }

        public bool Equals(Detail other)
        {
            return Name == other.Name;
        }
    }
}
