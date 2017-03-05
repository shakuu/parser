﻿using System;

using Bytes2you.Validation;

using Parser.Common.Contracts;
using Parser.Common.Factories;
using Parser.LogFileParser.Contracts;
using Parser.LogFileParser.EventsArgs;

namespace Parser.LogFileParser.Engines
{
    public class LogFileParserEngine : ILogFileParserEngine
    {
        public event EventHandler<ExitCombatEventArgs> OnExitCombat;

        private readonly ICommandResolutionHandler commandResolutionHandler;

        private ICombatStatisticsContainer combatStatisticsContainer;

        public LogFileParserEngine(ICommandResolutionHandler commandResolutionHandler, ICombatStatisticsContainerFactory combatStatisticsContainerFactory)
        {
            Guard.WhenArgument(commandResolutionHandler, nameof(ICommandResolutionHandler)).IsNull().Throw();
            Guard.WhenArgument(combatStatisticsContainerFactory, nameof(ICombatStatisticsContainerFactory)).IsNull().Throw();

            this.commandResolutionHandler = commandResolutionHandler;
            this.combatStatisticsContainer = combatStatisticsContainerFactory.CreateCombatStatisticsContainer();
        }

        protected ICombatStatisticsContainer CombatStatisticsContainer { get { return this.combatStatisticsContainer; } set { this.combatStatisticsContainer = value; } }

        public void EnqueueCommand(ICommand command)
        {
            Guard.WhenArgument(command, nameof(ICommand)).IsNull().Throw();

            this.combatStatisticsContainer = this.commandResolutionHandler.ResolveCommand(command, this.combatStatisticsContainer);
        }

        public ICombatStatisticsContainer GetComabtStatistics()
        {
            return this.combatStatisticsContainer;
        }
    }
}
