using Moq;
using Xunit;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google;

public class GmailServiceHelperTests
{
    private readonly Mock<IGmailApiWrapper> _mockApiWrapper;
    private readonly GmailServiceHelper _serviceHelper;
    private readonly GmailService _dummyService;

    public GmailServiceHelperTests()
    {
        _mockApiWrapper = new Mock<IGmailApiWrapper>();
        _serviceHelper = new GmailServiceHelper(_mockApiWrapper.Object);
        _dummyService = new GmailService(new BaseClientService.Initializer());
    }

    [Fact]
    public async Task SearchMessages_Success_ReturnsMessages()
    {
        // Arrange
        var expectedMessages = new List<Message>
        {
            new Message { Id = "1", Snippet = "This is a test email" },
            new Message { Id = "2", Snippet = "Another email about testing" },
            new Message { Id = "3", Snippet = "Irrelevant content" }
        };
        // Simulate filtering: only return messages containing 'test'
        _mockApiWrapper
            .Setup(w => w.SearchMessages(It.IsAny<UsersResource.MessagesResource.ListRequest>()))
            .ReturnsAsync(expectedMessages.Where(m => m.Snippet.Contains("test")).ToList());

        // Act
        var result = await _serviceHelper.SearchMessages(_dummyService, "test");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, m => Assert.Contains("test", m.Snippet));
        _mockApiWrapper.Verify(w => w.SearchMessages(It.IsAny<UsersResource.MessagesResource.ListRequest>()), Times.Once);
    }

    [Fact]
    public async Task SearchMessages_GoogleApiException_ReturnsEmptyList()
    {
        // Arrange
        _mockApiWrapper
            .Setup(w => w.SearchMessages(It.IsAny<UsersResource.MessagesResource.ListRequest>()))
            .ThrowsAsync(new GoogleApiException("", "Test error"));

        // Act
        var result = await _serviceHelper.SearchMessages(_dummyService, "test");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMessage_Success_ReturnsMessage()
    {
        // Arrange
        var expectedMessage = new Message();
        _mockApiWrapper
            .Setup(w => w.GetMessage(It.IsAny<UsersResource.MessagesResource.GetRequest>()))
            .ReturnsAsync(expectedMessage);

        // Act
        var result = await _serviceHelper.GetMessage(_dummyService, "msg_123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedMessage, result);
    }

    [Fact]
    public async Task GetMessage_GoogleApiException_ReturnsNull()
    {
        // Arrange
        _mockApiWrapper
            .Setup(w => w.GetMessage(It.IsAny<UsersResource.MessagesResource.GetRequest>()))
            .ThrowsAsync(new GoogleApiException("", "Message not found"));

        // Act
        var result = await _serviceHelper.GetMessage(_dummyService, "invalid_id");

        // Assert
        Assert.Null(result);
    }
}