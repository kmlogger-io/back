using System;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Category : Entity
    {
        public UniqueName? Name { get; private set; }
        public bool? Active { get; private set; }
        public List<App>? Apps { get; private set; }

        private Category() { }

        public Category(UniqueName? name, bool? active)
        {
            AddNotificationsFromValueObjects(name);
            Name = name;
            Active = active;
            Apps = new List<App>();
        }
    }
}
