﻿using h0wXD.Diagnostics.Domain;

namespace h0wXD.Diagnostics.Interfaces
{
    public interface IStealthProcess
    {
        ProcessOutput Execute(ProcessArguments _processArguments);
    }
}
