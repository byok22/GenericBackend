namespace Tests;

public class InsertRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly InsertRoleUseCase _useCase;

    public InsertRoleUseCaseTests()
    {
        _mockRepository = new Mock<IRoleRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new InsertRoleUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse_WhenRoleIsCreated()
    {
        // Arrange
        var roleDto = new RoleDto { PKRole = 0, RoleName = "Manager", Available = true };
        var roleModel = new Domain.Models.Role { PKRole = 0, RoleName = "Manager", Available = true };
        var createdRole = new Domain.Models.Role { PKRole = 1, RoleName = "Manager", Available = true };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.AddAsync(roleModel)).ReturnsAsync(createdRole);

        // Act
        var result = await _useCase.Execute(roleDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Contain("successfully");
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Execute_ShouldReturnFailureResponse_WhenRoleCreationFails()
    {
        // Arrange
        var roleDto = new RoleDto { RoleName = "Editor" };
        var roleModel = new Domain.Models.Role { RoleName = "Editor" };
        var failedRole = new Domain.Models.Role { PKRole = 0, RoleName = "Editor" };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.AddAsync(roleModel)).ReturnsAsync(failedRole);

        // Act
        var result = await _useCase.Execute(roleDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.Id.Should().Be(0);
    }

    [Fact]
    public async Task Execute_ShouldCallMapperAndRepository()
    {
        // Arrange
        var roleDto = new RoleDto { RoleName = "Viewer" };
        var roleModel = new Domain.Models.Role { RoleName = "Viewer" };
        var createdRole = new Domain.Models.Role { PKRole = 2, RoleName = "Viewer" };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.AddAsync(roleModel)).ReturnsAsync(createdRole);

        // Act
        await _useCase.Execute(roleDto);

        // Assert
        _mockMapper.Verify(m => m.Map<Domain.Models.Role>(roleDto), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(roleModel), Times.Once);
    }
}

public class UpdateRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateRoleUseCase _useCase;

    public UpdateRoleUseCaseTests()
    {
        _mockRepository = new Mock<IRoleRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new UpdateRoleUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse_WhenRoleIsUpdated()
    {
        // Arrange
        var roleDto = new RoleDto { PKRole = 1, RoleName = "Manager", Available = true };
        var roleModel = new Domain.Models.Role { PKRole = 1, RoleName = "Manager", Available = true };
        var dbResponse = new Shared.Response.DBResponse { id = 1, message = "Updated" };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.UpdateAsync(roleModel)).ReturnsAsync(dbResponse);

        // Act
        var result = await _useCase.Execute(roleDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_ShouldCallRepositoryUpdate()
    {
        // Arrange
        var roleDto = new RoleDto { PKRole = 1, RoleName = "Admin" };
        var roleModel = new Domain.Models.Role { PKRole = 1, RoleName = "Admin" };
        var dbResponse = new Shared.Response.DBResponse { id = 1 };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.UpdateAsync(roleModel)).ReturnsAsync(dbResponse);

        // Act
        await _useCase.Execute(roleDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(roleModel), Times.Once);
    }
}

public class DeleteRoleUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteRoleUseCase _useCase;

    public DeleteRoleUseCaseTests()
    {
        _mockRepository = new Mock<IRoleRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new DeleteRoleUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnSuccessResponse_WhenRoleIsDeleted()
    {
        // Arrange
        var roleDto = new RoleDto { PKRole = 1, RoleName = "TempRole" };
        var roleModel = new Domain.Models.Role { PKRole = 1, RoleName = "TempRole" };
        var dbResponse = new Shared.Response.DBResponse { id = 1, message = "Deleted" };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.RemoveAsync(roleModel)).ReturnsAsync(dbResponse);

        // Act
        var result = await _useCase.Execute(roleDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_ShouldReturnFailureResponse_WhenRoleDeletionFails()
    {
        // Arrange
        var roleDto = new RoleDto { PKRole = 999, RoleName = "NonExistent" };
        var roleModel = new Domain.Models.Role { PKRole = 999, RoleName = "NonExistent" };
        var dbResponse = new Shared.Response.DBResponse { id = 0, message = "Not found" };

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(roleModel);
        _mockRepository.Setup(r => r.RemoveAsync(roleModel)).ReturnsAsync(dbResponse);

        // Act
        var result = await _useCase.Execute(roleDto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenRoleDtoIsNull()
    {
        // Arrange
        var roleDto = new RoleDto { PKRole = 1, RoleName = "TestRole" };
        Domain.Models.Role? nullRole = null;

        _mockMapper.Setup(m => m.Map<Domain.Models.Role>(roleDto)).Returns(nullRole);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _useCase.Execute(roleDto));
    }
}

public class GetRoleByIdUseCaseTests
{
    private readonly Mock<IRoleRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetRoleByIdUseCase _useCase;

    public GetRoleByIdUseCaseTests()
    {
        _mockRepository = new Mock<IRoleRepository>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new GetRoleByIdUseCase(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnRoleDto_WhenRoleExists()
    {
        // Arrange
        int roleId = 1;
        var roleModel = new Domain.Models.Role { PKRole = 1, RoleName = "Admin", Available = true };
        var roleDto = new RoleDto { PKRole = 1, RoleName = "Admin", Available = true };

        _mockRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(roleModel);
        _mockMapper.Setup(m => m.Map<RoleDto>(roleModel)).Returns(roleDto);

        // Act
        var result = await _useCase.Execute(roleId);

        // Assert
        result.Should().NotBeNull();
        result.PKRole.Should().Be(1);
        result.RoleName.Should().Be("Admin");
    }

    [Fact]
    public async Task Execute_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        int roleId = 5;
        var roleModel = new Domain.Models.Role { PKRole = 5, RoleName = "Editor" };
        var roleDto = new RoleDto { PKRole = 5, RoleName = "Editor" };

        _mockRepository.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(roleModel);
        _mockMapper.Setup(m => m.Map<RoleDto>(roleModel)).Returns(roleDto);

        // Act
        await _useCase.Execute(roleId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(roleId), Times.Once);
    }
}
