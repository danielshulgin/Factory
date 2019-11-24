using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Domain
{
    public class DetailType
    {
        public string Name { get; private set; }

        public DetailType(string name)
        {
            Name = name;
        }
    }
}
