using System;
using System.Collections.Generic;
using Domain.ValueObjects;
using Domain.Enums;
using Environment = Domain.Enums.Environment;

namespace Domain.Entities
{
    public class App : Entity
    {
        public UniqueName Name { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public Environment? Environment { get; private set; }
        public List<LogApp?>? Logs { get; private set; }
        public bool? Active { get; private set; }

        private App() { }

        public App(UniqueName name, Category category, Environment? environment,
         List<LogApp>? logs = null, bool? active = true)
        {
            Name = name;
            Category = category;
            Environment = environment;
            Logs = logs;
            Active = active;
        }
    }
}
