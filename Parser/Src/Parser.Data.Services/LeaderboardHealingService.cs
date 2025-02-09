﻿using System.Collections.Generic;

using Bytes2you.Validation;

using Parser.Common.Html.Svg;
using Parser.Data.Contracts;
using Parser.Data.Services.Contracts;
using Parser.Data.ViewModels.Factories;
using Parser.Data.ViewModels.Leaderboard;

namespace Parser.Data.Services
{
    public class LeaderboardHealingService : ILeaderboardHealingService
    {
        private const int DefaultSvgElementSize = 300;
        private const int DefaultPercentageBarRadius = 75;
        private const int DefaultPageSize = 5;
        private const int DefaultPageNumber = 1;

        private readonly IHealingViewModelDataProvider healingViewModelDataProvider;
        private readonly IHealingViewModelFactory healingViewModelFactory;
        private readonly IPartialCircleSvgPathProvider partialCircleSvgPathProvider;

        public LeaderboardHealingService(IHealingViewModelDataProvider healingViewModelDataProvider, IHealingViewModelFactory healingViewModelFactory, IPartialCircleSvgPathProvider partialCircleSvgPathProvider)
        {
            Guard.WhenArgument(healingViewModelDataProvider, nameof(IHealingViewModelDataProvider)).IsNull().Throw();
            Guard.WhenArgument(healingViewModelFactory, nameof(IHealingViewModelFactory)).IsNull().Throw();
            Guard.WhenArgument(partialCircleSvgPathProvider, nameof(IPartialCircleSvgPathProvider)).IsNull().Throw();

            this.healingViewModelDataProvider = healingViewModelDataProvider;
            this.healingViewModelFactory = healingViewModelFactory;
            this.partialCircleSvgPathProvider = partialCircleSvgPathProvider;
        }

        public HealingViewModel GetTopStoredHealingOnPage(int pageNumber)
        {
            if (pageNumber <= 0)
            {
                pageNumber = LeaderboardHealingService.DefaultPageNumber;
            }

            var healingViewModel = new List<HealingViewModel>();
            for (int pageIndex = 1; pageIndex <= pageNumber; pageIndex++)
            {
                var resultHealingViewModel = this.healingViewModelDataProvider.GetHealingViewModelOnPage(pageIndex, LeaderboardHealingService.DefaultPageSize);
                healingViewModel.Add(resultHealingViewModel);
            }

            var healingDonePersecondViewModels = new List<HealingDonePerSecondViewModel>();
            foreach (var viewModel in healingViewModel)
            {
                healingDonePersecondViewModels.AddRange(viewModel.HealingDonePerSecondViewModels);
            }

            pageNumber = healingDonePersecondViewModels.Count / LeaderboardHealingService.DefaultPageSize;
            var damageViewModel = this.healingViewModelFactory.CreateHealingViewModell(pageNumber, healingViewModel[0].MaximumHealingDonePerSecond, healingDonePersecondViewModels);

            foreach (var viewModel in damageViewModel.HealingDonePerSecondViewModels)
            {
                viewModel.SvgString = this.partialCircleSvgPathProvider.GetSvgPath(viewModel.PercentageOfBest, LeaderboardHealingService.DefaultPercentageBarRadius, LeaderboardHealingService.DefaultSvgElementSize);
            }

            return damageViewModel;
        }
    }
}
