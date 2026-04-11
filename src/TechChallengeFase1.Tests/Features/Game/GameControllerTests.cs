using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallengeFase1.Api.Controllers;
using TechChallengeFase1.Application.DTOs;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Tests.Builders;

namespace TechChallengeFase1.Tests.Features.Game;

public class GameControllerTests
{
    private readonly Mock<IGameService> _gameServiceMock;
    private readonly GameController _sut;

    public GameControllerTests()
    {
        _gameServiceMock = new Mock<IGameService>();
        _sut = new GameController(_gameServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_Returns201Created()
    {
        // Arrange
        var dto = new GameBuilder().BuildDto();
        var createdGame = new GameBuilder().WithGameId(1).BuildOutputDto();

        _gameServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<GameInputDto>()))
            .ReturnsAsync(createdGame);

        // Act
        var actionResult = await _sut.CreateAsync(dto);

        // Assert
        var result = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(201, result.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsGameOutputDtoInBody()
    {
        // Arrange
        var dto = new GameBuilder().WithTitle("Hollow Knight").WithBasePrice(39.99m).BuildDto();
        var createdGame = new GameBuilder().WithGameId(7).WithTitle("Hollow Knight").WithBasePrice(39.99m).BuildOutputDto();

        _gameServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<GameInputDto>()))
            .ReturnsAsync(createdGame);

        // Act
        var actionResult = await _sut.CreateAsync(dto);

        // Assert
        var result = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var output = Assert.IsType<GameOutputDto>(result.Value);

        Assert.Equal(7, output.GameId);
        Assert.Equal("Hollow Knight", output.Title);
        Assert.Equal(39.99m, output.BasePrice);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_RouteValueContainsNewGameId()
    {
        // Arrange
        var dto = new GameBuilder().BuildDto();
        var createdGame = new GameBuilder().WithGameId(42).BuildOutputDto();

        _gameServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<GameInputDto>()))
            .ReturnsAsync(createdGame);

        // Act
        var actionResult = await _sut.CreateAsync(dto);

        // Assert
        var result = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(42, result.RouteValues!["id"]);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_CallsServiceExactlyOnce()
    {
        // Arrange
        var dto = new GameBuilder().BuildDto();
        var createdGame = new GameBuilder().BuildOutputDto();

        _gameServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<GameInputDto>()))
            .ReturnsAsync(createdGame);

        // Act
        await _sut.CreateAsync(dto);

        // Assert
        _gameServiceMock.Verify(s => s.CreateAsync(It.IsAny<GameInputDto>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_MapsAllFieldsFromGameToOutput()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var releaseDate = new DateTime(2020, 9, 17);

        var dto = new GameBuilder()
            .WithCreatedBy(createdBy)
            .BuildDto();

        var createdGame = new GameBuilder()
            .WithGameId(5)
            .WithTitle("Hades")
            .WithBasePrice(24.99m)
            .WithCreatedBy(createdBy)
            .BuildOutputDto();

        _gameServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<GameInputDto>()))
            .ReturnsAsync(createdGame);

        // Act
        var actionResult = await _sut.CreateAsync(dto);

        // Assert
        var result = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var output = Assert.IsType<GameOutputDto>(result.Value);

        Assert.Equal(createdGame.GameId, output.GameId);
        Assert.Equal(createdGame.Title, output.Title);
        Assert.Equal(createdGame.Description, output.Description);
        Assert.Equal(createdGame.DeveloperId, output.DeveloperId);
        Assert.Equal(createdGame.PublisherId, output.PublisherId);
        Assert.Equal(createdGame.BasePrice, output.BasePrice);
        Assert.Equal(createdGame.IsActive, output.IsActive);
        Assert.Equal(createdGame.CreatedBy, output.CreatedBy);
    }

    [Fact]
    public async Task CreateAsync_WhenServiceThrowsArgumentException_ExceptionPropagates()
    {
        // Arrange — a validação é responsabilidade do serviço;
        // o middleware captura e retorna 400. Aqui garantimos que a exceção não é engolida pelo controller.
        var dto = new GameBuilder().WithTitle("").BuildDto();

        _gameServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<GameInputDto>()))
            .ThrowsAsync(new ArgumentException("O título do jogo é obrigatório."));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.CreateAsync(dto));
    }
}
