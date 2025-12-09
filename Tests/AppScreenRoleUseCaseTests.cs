namespace Tests;

public class GetAppScreenRolesByRoleUseCaseTests
{
    private readonly Mock<IAppScreenRoleRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAppScreenRolesByRoleUseCase _useCase;

    public GetAppScreenRolesByRoleUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreenRoleRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new GetAppScreenRolesByRoleUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnAppScreenRoles_WhenRoleIdExists()
    {
        // Arrange
        int roleId = 1;
        var appScreenRoles = new List<Domain.Models.AppScreenRoleDetail>
        {
            new Domain.Models.AppScreenRoleDetail 
            { 
                PKScreenRoles = 1, 
                FKScreen = 1, 
                FKRoles = roleId,
                ScreenName = "Dashboard",
                ScreenPath = "/dashboard"
            },
            new Domain.Models.AppScreenRoleDetail 
            { 
                PKScreenRoles = 2, 
                FKScreen = 2, 
                FKRoles = roleId,
                ScreenName = "Settings",
                ScreenPath = "/settings"
            }
        };

        var expectedDtos = new List<AppScreenRoleDTO>
        {
            new AppScreenRoleDTO { PKScreenRoles = 1, FKScreen = 1, FKRoles = roleId },
            new AppScreenRoleDTO { PKScreenRoles = 2, FKScreen = 2, FKRoles = roleId }
        };

        _mockRepository.Setup(r => r.GetByRoleIdAsync(roleId)).ReturnsAsync(appScreenRoles);
        _mockMapper.Setup(m => m.Map<List<AppScreenRoleDTO>>(appScreenRoles)).Returns(expectedDtos);

        // Act
        var result = await _useCase.Execute(roleId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].FKRoles.Should().Be(roleId);
        result[1].FKRoles.Should().Be(roleId);
        _mockRepository.Verify(r => r.GetByRoleIdAsync(roleId), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenRoleHasNoScreens()
    {
        // Arrange
        int roleId = 999;
        var emptyList = new List<Domain.Models.AppScreenRoleDetail>();
        var expectedDtos = new List<AppScreenRoleDTO>();

        _mockRepository.Setup(r => r.GetByRoleIdAsync(roleId)).ReturnsAsync(emptyList);
        _mockMapper.Setup(m => m.Map<List<AppScreenRoleDTO>>(emptyList)).Returns(expectedDtos);

        // Act
        var result = await _useCase.Execute(roleId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Execute_ShouldCallRepositoryWithCorrectRoleId()
    {
        // Arrange
        int roleId = 5;
        var appScreenRoles = new List<Domain.Models.AppScreenRoleDetail>();
        var expectedDtos = new List<AppScreenRoleDTO>();

        _mockRepository.Setup(r => r.GetByRoleIdAsync(roleId)).ReturnsAsync(appScreenRoles);
        _mockMapper.Setup(m => m.Map<List<AppScreenRoleDTO>>(appScreenRoles)).Returns(expectedDtos);

        // Act
        await _useCase.Execute(roleId);

        // Assert
        _mockRepository.Verify(r => r.GetByRoleIdAsync(roleId), Times.Once);
    }
}

public class SyncPermissionsForRoleUseCaseTests
{
    private readonly Mock<IAppScreenRoleRepository> _mockRepository;
    private readonly SyncPermissionsForRoleUseCase _useCase;

    public SyncPermissionsForRoleUseCaseTests()
    {
        _mockRepository = new Mock<IAppScreenRoleRepository>();
        _useCase = new SyncPermissionsForRoleUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldSyncPermissions_WithValidScreenIds()
    {
        // Arrange
        var syncDto = new SyncPermissionsDto 
        { 
            RoleId = 1, 
            ScreenIds = new List<int> { 1, 2, 3 }
        };

        _mockRepository.Setup(r => r.SyncPermissionsForRoleAsync(syncDto.RoleId, syncDto.ScreenIds))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(syncDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Contain("synchronized");
        _mockRepository.Verify(r => r.SyncPermissionsForRoleAsync(syncDto.RoleId, syncDto.ScreenIds), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldSyncPermissions_WithEmptyScreenIds()
    {
        // Arrange
        var syncDto = new SyncPermissionsDto 
        { 
            RoleId = 1, 
            ScreenIds = new List<int>()
        };

        _mockRepository.Setup(r => r.SyncPermissionsForRoleAsync(syncDto.RoleId, syncDto.ScreenIds))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(syncDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        _mockRepository.Verify(r => r.SyncPermissionsForRoleAsync(syncDto.RoleId, syncDto.ScreenIds), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse()
    {
        // Arrange
        var syncDto = new SyncPermissionsDto 
        { 
            RoleId = 2, 
            ScreenIds = new List<int> { 5, 10, 15 }
        };

        _mockRepository.Setup(r => r.SyncPermissionsForRoleAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(syncDto);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
    }
}
