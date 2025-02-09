﻿using Moq;
using NUnit.Framework;

using Parser.Common.Contracts;
using Parser.LogFile.Parser.Contracts;
using Parser.LogFile.SignalR.Services;

namespace Parser.LogFile.SignalR.Tests.ServicesTests.LogFileParserHubServiceTests
{
    [TestFixture]
    public class ReleaseParsingSessionId_Should
    {
        [Test]
        public void ShouldInvokeILogFileParserEngineManager_ReleaseParsingSessionMethodOnceWithCorrectParameter()
        {
            // Arrange
            var logFileParserEngineManager = new Mock<ILogFileParserEngineManager>();
            var commandJsonConvertProvider = new Mock<ICommandEnumerationJsonConvertProvider>();

            var logFileParserHubService = new LogFileParserHubService(logFileParserEngineManager.Object, commandJsonConvertProvider.Object);

            var engineId = "any engine id";

            // Act
            logFileParserHubService.ReleaseParsingSessionId(engineId);

            // Assert
            logFileParserEngineManager.Verify(m => m.StopLogFileParserEngine(engineId), Times.Once);
        }
    }
}
