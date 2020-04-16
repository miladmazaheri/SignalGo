﻿using System;
using SignalGo.Publisher.Engines.Interfaces;
using SignalGo.Shared.Log;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SignalGo.Publisher.Engines.Models;

namespace SignalGo.Publisher.Engines.Commands
{
    public class QueueCommandInfo : CommandBaseInfo
    {
        public List<ICommand> Commands { get; set; }
        public bool IsSuccess { get; set; } = false;

        public QueueCommandInfo(IEnumerable<ICommand> commands)
        {
            Commands = commands.ToList();
        }

        public override async Task<Process> Run(CancellationToken cancellationToken)
        {

            var proc = new Process();
            Status = RunStatusType.Running;
            try
            {
                foreach (var item in Commands)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Debug.WriteLine($"Cancellation Requested in task {Task.CurrentId}");
                        Status = RunStatusType.Cancelled;
                        return proc;
                    }
                    proc = await item.Run(cancellationToken);
                    if (proc.ExitCode != 0)
                    {
                        IsSuccess = false;
                        Status = RunStatusType.Error;
                        return proc;
                    }
                }
            }
            catch (Exception ex)
            {
                AutoLogger.Default.LogError(ex, "QueueRunner");
            }
            Status = RunStatusType.Done;
            return proc;
        }
    }
}
