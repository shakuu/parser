﻿using Parser.Common.Logging;

namespace Parser.Data.ViewModels.Administration
{
    public class AdministrationIndexViewModel
    {
        public AdministrationIndexViewModel()
        {
            this.MessageType = MessageType.Error;
            this.PeriodType = PeriodType.Day;
        }

        public MessageType MessageType { get; set; }

        public PeriodType PeriodType { get; set; }
    }
}
