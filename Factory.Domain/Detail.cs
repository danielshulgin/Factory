using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public struct Detail : IEquatable<Detail>
    {
        public string Name { get; private set; }

        public Detail(string name)
        {
            Name = name;
        }

        public bool Equals(Detail other)
        {
            return Name == other.Name;
        }
    }
}
