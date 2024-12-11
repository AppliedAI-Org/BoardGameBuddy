﻿using System.Threading.Tasks;
using AiPocWebsiteTemplateWithBackend.Business;
using AiPocWebsiteTemplateWithBackend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AiPocWebsiteTemplateWithBackend.Tests.Unit.Controllers
{
    public class FunctionControllerTests
    {
        private readonly Mock<IAuthLogic> _mockAuthLogic;
        private readonly Mock<IPromptFlowLogic> _mockPromptFlowLogic;
        private readonly FunctionController _functionController;

        public FunctionControllerTests()
        {
            _mockAuthLogic = new Mock<IAuthLogic>();
            _mockPromptFlowLogic = new Mock<IPromptFlowLogic>();
            _functionController = new FunctionController(_mockAuthLogic.Object, _mockPromptFlowLogic.Object);
        }
    }
}