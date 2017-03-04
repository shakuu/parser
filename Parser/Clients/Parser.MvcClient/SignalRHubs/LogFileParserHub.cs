﻿using Microsoft.AspNet.SignalR;

using Ninject;

using Parser.MvcClient.App_Start;
using Parser.SignalR.Contracts;

namespace Parser.MvcClient.SignalRHubs
{
    public class LogFileParserHub : Hub, ILogFileParserHub
    {
        private readonly ILogFileParserHubService logFileParserHubService;

        public LogFileParserHub()
        {
            this.logFileParserHubService = NinjectWebCommon.Kernel.Get<ILogFileParserHubService>();
        }

        public void SendCommand(string engineId, string serializedCommand)
        {
            var message = this.logFileParserHubService.SendCommand(engineId, serializedCommand);

            Clients.Caller.UpdateStatus(message);
        }

        public void EndParsingSession(string engineId)
        {
            this.logFileParserHubService.ReleaseParsingSessionId(engineId);
        }

        public void GetParsingSessionId()
        {
            var username = Context.User.Identity.Name;
            var message = this.logFileParserHubService.GetParsingSessionId(username);
            
            Clients.Caller.UpdateParsingSessionId(message);
        }
    }
}