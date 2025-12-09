namespace Tests;

public class GetAllAppScreensUseCaseTests
{
    private readonly Mock<IAppScreensRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllAppScreensUseCase _useCase;

    public GetAllAppScreensUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreensRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new GetAllAppScreensUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedAppScreens_WhenRepositoryReturnsData()
    {
        // Arrange
        var appScreens = new List<Domain.Models.AppScreen>
        {
            new Domain.Models.AppScreen { AppScreenID = 1, Screen = "Screen1", Url = "/screen1" },
            new Domain.Models.AppScreen { AppScreenID = 2, Screen = "Screen2", Url = "/screen2" }
        };

        var expectedDtos = new List<AppScreenDto>
        {
            new AppScreenDto { AppScreenID = 1, Screen = "Screen1", Url = "/screen1" },
            new AppScreenDto { AppScreenID = 2, Screen = "Screen2", Url = "/screen2" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(appScreens);
        _mockMapper.Setup(m => m.Map<List<AppScreenDto>>(appScreens)).Returns(expectedDtos);

        // Act
        var result = await _useCase.Execute();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Screen.Should().Be("Screen1");
        result[1].Screen.Should().Be("Screen2");
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        var emptyList = new List<Domain.Models.AppScreen>();
        var expectedDtos = new List<AppScreenDto>();

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyList);
        _mockMapper.Setup(m => m.Map<List<AppScreenDto>>(emptyList)).Returns(expectedDtos);

        // Act
        var result = await _useCase.Execute();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Execute_ShouldCallMapperWithRepositoryData()
    {
        // Arrange
        var appScreens = new List<Domain.Models.AppScreen>
        {
            new Domain.Models.AppScreen { AppScreenID = 1, Screen = "Admin Panel" }
        };
        var expectedDtos = new List<AppScreenDto>
        {
            new AppScreenDto { AppScreenID = 1, Screen = "Admin Panel" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(appScreens);
        _mockMapper.Setup(m => m.Map<List<AppScreenDto>>(appScreens)).Returns(expectedDtos);

        // Act
        await _useCase.Execute();

        // Assert
        _mockMapper.Verify(m => m.Map<List<AppScreenDto>>(appScreens), Times.Once);
    }
}

public class GetAllRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllRoleUseCase _useCase;

    public GetAllRoleUseCaseTests()
    {
        _mockRepository = new Mock<IRoleRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new GetAllRoleUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedRoles_WhenRepositoryReturnsData()
    {
        // Arrange
        var roles = new List<Domain.Models.Role>
        {
            new Domain.Models.Role { PKRole = 1, RoleName = "Admin", Available = true },
            new Domain.Models.Role { PKRole = 2, RoleName = "User", Available = true }
        };

        var expectedDtos = new List<RoleDto>
        {
            new RoleDto { PKRole = 1, RoleName = "Admin", Available = true },
            new RoleDto { PKRole = 2, RoleName = "User", Available = true }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);
        _mockMapper.Setup(m => m.Map<List<RoleDto>>(roles)).Returns(expectedDtos);

        // Act
        var result = await _useCase.Execute();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].RoleName.Should().Be("Admin");
        result[1].RoleName.Should().Be("User");
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        var emptyList = new List<Domain.Models.Role>();
        var expectedDtos = new List<RoleDto>();

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyList);
        _mockMapper.Setup(m => m.Map<List<RoleDto>>(emptyList)).Returns(expectedDtos);

        // Act
        var result = await _useCase.Execute();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Execute_ShouldCallMapperWithRepositoryData()
    {
        // Arrange
        var roles = new List<Domain.Models.Role>
        {
            new Domain.Models.Role { PKRole = 1, RoleName = "Admin" }
        };
        var expectedDtos = new List<RoleDto>
        {
            new RoleDto { PKRole = 1, RoleName = "Admin" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(roles);
        _mockMapper.Setup(m => m.Map<List<RoleDto>>(roles)).Returns(expectedDtos);

        // Act
        await _useCase.Execute();

        // Assert
        _mockMapper.Verify(m => m.Map<List<RoleDto>>(roles), Times.Once);
    }
}