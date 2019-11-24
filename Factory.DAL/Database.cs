using System;
using System.Collections.Generic;
using Factory.Domain;

namespace Factory.DAL
{
    public class Database
    {
        private List<DetailType> detailsTypes;
        public Database()
        {
            detailsTypes = new List<DetailType>();        
        }

        public IReadOnlyCollection<DetailType> GetAllDetailsTypes()
        {
            return detailsTypes.ToArray();
        }

        public void AddDetailType(DetailType detail)
        {
            detailsTypes.Add(detail);
        }
    }
}
