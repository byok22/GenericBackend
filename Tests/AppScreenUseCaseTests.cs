namespace Tests;

public class CreateAppScreenUseCaseTests
{
    private readonly Mock<IAppScreensRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateAppScreenUseCase _useCase;

    public CreateAppScreenUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreensRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new CreateAppScreenUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse_WhenAppScreenIsCreated()
    {
        // Arrange
        var appScreenDto = new AppScreenDto 
        { 
            AppScreenID = 0, 
            Screen = "Dashboard", 
            Url = "/dashboard",
            Available = true 
        };
        var appScreenModel = new Domain.Models.AppScreen 
        { 
            AppScreenID = 0, 
            Screen = "Dashboard", 
            Url = "/dashboard"
        };
        var createdAppScreen = new Domain.Models.AppScreen 
        { 
            AppScreenID = 1, 
            Screen = "Dashboard", 
            Url = "/dashboard"
        };

        _mockMapper.Setup(m => m.Map<Domain.Models.AppScreen>(appScreenDto)).Returns(appScreenModel);
        _mockRepository.Setup(r => r.AddAsync(appScreenModel)).ReturnsAsync(createdAppScreen);

        // Act
        var result = await _useCase.Execute(appScreenDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Contain("successfully");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Execute_ShouldReturnFailureResponse_WhenCreationFails()
    {
        // Arrange
        var appScreenDto = new AppScreenDto { Screen = "Test" };
        var appScreenModel = new Domain.Models.AppScreen { Screen = "Test" };
        var failedAppScreen = new Domain.Models.AppScreen { AppScreenID = 0 };

        _mockMapper.Setup(m => m.Map<Domain.Models.AppScreen>(appScreenDto)).Returns(appScreenModel);
        _mockRepository.Setup(r => r.AddAsync(appScreenModel)).ReturnsAsync(failedAppScreen);

        // Act
        var result = await _useCase.Execute(appScreenDto);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Id.Should().Be(0);
    }
}

public class EditAppScreenUseCaseTests
{
    private readonly Mock<IAppScreensRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EditAppScreenUseCase _useCase;

    public EditAppScreenUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreensRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new EditAppScreenUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse_WhenAppScreenIsUpdated()
    {
        // Arrange
        var appScreenDto = new AppScreenDto 
        { 
            AppScreenID = 1, 
            Screen = "Updated Dashboard", 
            Url = "/updated-dashboard"
        };
        var appScreenModel = new Domain.Models.AppScreen 
        { 
            AppScreenID = 1, 
            Screen = "Updated Dashboard"
        };
        var dbResponse = new Shared.Response.DBResponse { id = 1, message = "Updated" };

        _mockMapper.Setup(m => m.Map<Domain.Models.AppScreen>(appScreenDto)).Returns(appScreenModel);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Domain.Models.AppScreen>())).Returns(Task.FromResult(dbResponse));

        // Act
        var result = await _useCase.Execute(appScreenDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Contain("Edited");
    }

    [Fact]
    public async Task Execute_ShouldReturnFailureResponse_WhenUpdateFails()
    {
        // Arrange
        var appScreenDto = new AppScreenDto { AppScreenID = 999, Screen = "Test" };
        var appScreenModel = new Domain.Models.AppScreen { AppScreenID = 999 };
        var dbResponse = new Shared.Response.DBResponse { id = 0 };

        _mockMapper.Setup(m => m.Map<Domain.Models.AppScreen>(appScreenDto)).Returns(appScreenModel);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Domain.Models.AppScreen>())).Returns(Task.FromResult(dbResponse));

        // Act
        var result = await _useCase.Execute(appScreenDto);

        // Assert
        result.IsSuccessful.Should().BeFalse();
    }
}

public class DeleteAppScreenUseCaseTests
{
    private readonly Mock<IAppScreensRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteAppScreenUseCase _useCase;

    public DeleteAppScreenUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreensRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new DeleteAppScreenUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse_WhenAppScreenIsDeleted()
    {
        // Arrange
        var appScreenDto = new AppScreenDto { AppScreenID = 1, Screen = "ToDelete" };
        var appScreenModel = new Domain.Models.AppScreen { AppScreenID = 1 };
        var dbResponse = new Shared.Response.DBResponse { id = 1, message = "Deleted" };

        _mockMapper.Setup(m => m.Map<Domain.Models.AppScreen>(appScreenDto)).Returns(appScreenModel);
        _mockRepository.Setup(r => r.RemoveAsync(appScreenModel)).Returns(Task.FromResult(dbResponse));

        // Act
        var result = await _useCase.Execute(appScreenDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenAppScreenDtoIsNull()
    {
        // Arrange
        var appScreenDto = new AppScreenDto { AppScreenID = 1 };
        Domain.Models.AppScreen? nullAppScreen = null;

        _mockMapper.Setup(m => m.Map<Domain.Models.AppScreen>(appScreenDto)).Returns(nullAppScreen);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _useCase.Execute(appScreenDto));
    }
}

public class GetAppScreenByIdUseCaseTests
{
    private readonly Mock<IAppScreensRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAppScreenByIdUseCase _useCase;

    public GetAppScreenByIdUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreensRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new GetAppScreenByIdUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnAppScreenDto_WhenScreenExists()
    {
        // Arrange
        int screenId = 1;
        var appScreenModel = new Domain.Models.AppScreen 
        { 
            AppScreenID = 1, 
            Screen = "Dashboard", 
            Url = "/dashboard"
        };
        var appScreenDto = new AppScreenDto 
        { 
            AppScreenID = 1, 
            Screen = "Dashboard", 
            Url = "/dashboard"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(screenId)).ReturnsAsync(appScreenModel);
        _mockMapper.Setup(m => m.Map<AppScreenDto>(appScreenModel)).Returns(appScreenDto);

        // Act
        var result = await _useCase.Execute(screenId);

        // Assert
        result.Should().NotBeNull();
        result.AppScreenID.Should().Be(1);
        result.Screen.Should().Be("Dashboard");
    }

    [Fact]
    public async Task Execute_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        int screenId = 5;
        var appScreenModel = new Domain.Models.AppScreen { AppScreenID = 5 };
        var appScreenDto = new AppScreenDto { AppScreenID = 5 };

        _mockRepository.Setup(r => r.GetByIdAsync(screenId)).ReturnsAsync(appScreenModel);
        _mockMapper.Setup(m => m.Map<AppScreenDto>(appScreenModel)).Returns(appScreenDto);

        // Act
        await _useCase.Execute(screenId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(screenId), Times.Once);
    }
}
