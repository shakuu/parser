﻿using System;

using Parser.Common.Contracts;

namespace Parser.Common.Models
{
    public class Command : ICommand
    {
        public DateTime TimeStamp { get; set; }

        public string AbilityActivatorName { get; set; }

        public string AbilityTargetName { get; set; }

        public string AbilityName { get; set; }

        public string AbilityGameId { get; set; }

        public double AbilityCost { get; set; }

        public string EventType { get; set; }

        public string EventTypeGameId { get; set; }

        public string EventName { get; set; }

        public string EventNameGameId { get; set; }

        public double EffectAmount { get; set; }

        public string EffectType { get; set; }

        public string EffectTypeGameId { get; set; }

        public double EffectEffectiveAmount { get; set; }

        public bool IsCritical { get; set; }

        public string OriginalString { get; set; }
    }
}
