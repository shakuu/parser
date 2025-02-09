﻿using System;
using System.Collections.Generic;
using System.Linq;

using Bytes2you.Validation;

using Parser.Common.Contracts;
using Parser.Common.Factories;
using Parser.LogFile.Reader.Contracts;

namespace Parser.LogFile.Reader.Strategies
{
    public class CommandParsingStrategy : ICommandParsingStrategy
    {
        private readonly ICommandFactory commandFactory;

        public CommandParsingStrategy(ICommandFactory commandFactory)
        {
            Guard.WhenArgument(commandFactory, nameof(ICommandFactory)).IsNull().Throw();

            this.commandFactory = commandFactory;
        }

        public ICommand ParseCommand(string input)
        {
            Guard.WhenArgument(input, nameof(input)).IsNullOrEmpty().Throw();

            var commandWords = input.Split(new[] { '[', ']', '(', ')', '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
            commandWords = commandWords.Where(s => s != " ").ToArray();

            var nextCommand = this.commandFactory.CreateCommand();

            if (commandWords.Length == 0)
            {
                return nextCommand;
            }

            try
            {
                nextCommand.TimeStamp = this.GetTimeStamp(commandWords);
                nextCommand.AbilityActivatorName = this.GetAbilityActivatorName(commandWords);
                nextCommand.AbilityTargetName = this.GetAbilityTargetName(commandWords);

                nextCommand = this.GetAbilityDetails(commandWords, nextCommand);
                nextCommand = this.GetEventDetails(commandWords, nextCommand);
                nextCommand = this.GetEffectAmountDetails(commandWords, nextCommand);

                nextCommand.EffectEffectiveAmount = this.GetEffectEffectiveAmount(commandWords);

                nextCommand.OriginalString = input;
            }
            catch (Exception)
            {
                // TODO: Log Error
                return null;
            }

            return nextCommand;
        }

        private DateTime GetTimeStamp(IList<string> commandWords)
        {
            return DateTime.Parse(commandWords[0]);
        }

        private string GetAbilityActivatorName(IList<string> commandWords)
        {
            return commandWords[1];
        }

        private string GetAbilityTargetName(IList<string> commandWords)
        {
            return commandWords[2];
        }

        private ICommand GetAbilityDetails(IList<string> commandWords, ICommand command)
        {
            if (commandWords.Count < 4)
            {
                return command;
            }

            var abilityDetails = commandWords[3].Split(new[] { ':', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

            command.AbilityName = abilityDetails[0].Trim();
            command.AbilityGameId = abilityDetails[1].Trim();

            if (abilityDetails.Length > 2)
            {
                command.EventName = abilityDetails[2].Trim();
                command.EventNameGameId = abilityDetails[3];
            }

            return command;
        }

        private ICommand GetEventDetails(IList<string> commandWords, ICommand command)
        {
            if (commandWords.Count < 5)
            {
                return command;
            }

            var eventCommandWords = commandWords[4].Split(':');
            if (eventCommandWords.Length == 1)
            {
                command.AbilityCost = double.Parse(eventCommandWords[0]);
                return command;
            }

            var eventTypeDetails = eventCommandWords[0].Split(new[] { ':', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            command.EventType = eventTypeDetails[0].Trim();
            command.EventTypeGameId = eventTypeDetails[1];

            var eventNameDetails = eventCommandWords[1].Split(new[] { ':', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            command.EventName = eventNameDetails[0].Trim();
            command.EventNameGameId = eventNameDetails[1];

            return command;
        }

        private ICommand GetEffectAmountDetails(IList<string> commandWords, ICommand command)
        {
            if (commandWords.Count < 6)
            {
                return command;
            }

            var effectAmountDetails = commandWords[5].Split(new[] { ' ', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            if (effectAmountDetails[0].Contains("*"))
            {
                command.IsCritical = true;
                effectAmountDetails[0] = effectAmountDetails[0].Trim('*');
            }

            command.EffectAmount = double.Parse(effectAmountDetails[0]);

            if (effectAmountDetails.Length >= 2)
            {
                command.EffectType = effectAmountDetails[1];
            }

            if (effectAmountDetails.Length == 3)
            {
                command.EffectTypeGameId = effectAmountDetails[2];
            }

            return command;
        }

        private double GetEffectEffectiveAmount(IList<string> commandWords)
        {
            if (commandWords.Count >= 7)
            {
                return double.Parse(commandWords[6]);
            }

            return 0;
        }
    }
}
