using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.ValueObjects;

using Environment = Domain.Enums.Environment;

namespace Domain.Entities
{
    public class LogApp : Entity
    {
        public Environment Environment { get; private set; }  
        public Level? Level { get; private set; }  
        public Description? Message { get; private set; }
        public StackTrace? StackTrace { get; private set; }
        public Guid AppId { get; private set; }

        [NotMapped]
        public App App { get; private set; }

        private LogApp() { }

        public LogApp(Environment environment, Level? level, Description? message, StackTrace? stackTrace,
         Guid appId)
        {
            AddNotificationsFromValueObjects(message, stackTrace);
            Environment = environment;
            Level = level;
            Message = message;
            StackTrace = stackTrace;
            AppId = appId;
        }
    }
}
