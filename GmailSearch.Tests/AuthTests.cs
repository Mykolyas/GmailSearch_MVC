using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;


public class AuthControllerTests
{
    private readonly Mock<IConfiguration> _mockConfig = new();
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockConfig.Setup(c => c["GoogleAuth:ClientId"]).Returns("test_client_id");
        _mockConfig.Setup(c => c["GoogleAuth:ClientSecret"]).Returns("test_secret");
        _mockConfig.Setup(c => c["GoogleAuth:RedirectUri"]).Returns("https://test-callback");

        var httpContext = new DefaultHttpContext();
        var sessionMock = new Mock<ISession>();
        httpContext.Session = sessionMock.Object;

        _controller = new AuthController(_mockConfig.Object, _mockHttpClientFactory.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext }
        };
    }

    [Fact]
    public void Login_Generates_Correct_Redirect_URL()
    {
        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<RedirectResult>();
        var redirect = result as RedirectResult;
        redirect.Url.Should().Contain("https://accounts.google.com/o/oauth2/v2/auth");
        redirect.Url.Should().Contain("client_id=test_client_id");
        redirect.Url.Should().Contain("redirect_uri=https://test-callback");
        redirect.Url.Should().Contain("scope=https://www.googleapis.com/auth/gmail.readonly");
    }

    [Fact]
    public async Task Callback_MissingCode_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.Callback(null);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Missing authorization code");
    }


    [Fact]
    public async Task Logout_ClearsSession_RedirectsToHome()
    {
        // Arrange
        _controller.HttpContext.Session.SetString("AccessToken", "test_token");

        // Mocks for services
        var authServiceMock = new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationService>();
        var urlHelperFactoryMock = new Mock<Microsoft.AspNetCore.Mvc.Routing.IUrlHelperFactory>();
        var urlHelperMock = new Mock<Microsoft.AspNetCore.Mvc.IUrlHelper>();
        urlHelperMock.Setup(x => x.Action(It.IsAny<Microsoft.AspNetCore.Mvc.Routing.UrlActionContext>())).Returns("/");

        urlHelperFactoryMock
            .Setup(x => x.GetUrlHelper(It.IsAny<Microsoft.AspNetCore.Mvc.ActionContext>()))
            .Returns(urlHelperMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(Microsoft.AspNetCore.Authentication.IAuthenticationService)))
            .Returns(authServiceMock.Object);
        serviceProviderMock
            .Setup(x => x.GetService(typeof(Microsoft.AspNetCore.Mvc.Routing.IUrlHelperFactory)))
            .Returns(urlHelperFactoryMock.Object);

        _controller.HttpContext.RequestServices = serviceProviderMock.Object;

        // Act
        var result = await _controller.Logout();

        // Assert
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ControllerName.Should().Be("Home");

        _controller.HttpContext.Session.Keys.Should().BeEmpty();
    }
}