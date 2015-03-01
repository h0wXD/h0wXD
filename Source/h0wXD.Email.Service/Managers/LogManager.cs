﻿using h0wXD.Logging;
using h0wXD.Logging.Behaviors;

namespace h0wXD.Email.Service.Managers
{
    public class LogManager : Logger
    {
        public LogManager()
        {
            AddBehavior(new LogToFileBehavior());
        }
    }
}
