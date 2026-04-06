using Moq;
using TechChallengeFase1.Application.Services;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Tests.Builders;

namespace TechChallengeFase1.Tests.Features.Game;

public class CreateGameTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GameService _sut;

    public CreateGameTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _sut = new GameService(_gameRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ReturnsCreatedGame()
    {
        // Arrange
        var dto = new GameBuilder().BuildDto();
        var expectedGame = new GameBuilder().Build();

        _gameRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.Game>()))
            .ReturnsAsync(expectedGame);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Title, result.Title);
        Assert.Equal(dto.BasePrice, result.BasePrice);
        Assert.Equal(dto.DeveloperId, result.DeveloperId);
        _gameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.Game>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var dto = new GameBuilder().WithTitle("").BuildDto();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.CreateAsync(dto));

        Assert.Contains("título", exception.Message, StringComparison.OrdinalIgnoreCase);
        _gameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.Game>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithWhitespaceTitle_ThrowsArgumentException()
    {
        // Arrange
        var dto = new GameBuilder().WithTitle("   ").BuildDto();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_WithNegativeBasePrice_ThrowsArgumentException()
    {
        // Arrange
        var dto = new GameBuilder().WithBasePrice(-1m).BuildDto();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.CreateAsync(dto));

        Assert.Contains("preço", exception.Message, StringComparison.OrdinalIgnoreCase);
        _gameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.Game>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithZeroBasePrice_CreatesGame()
    {
        // Arrange — preço zero é válido (jogo gratuito)
        var dto = new GameBuilder().WithBasePrice(0m).BuildDto();
        var expectedGame = new GameBuilder().WithBasePrice(0m).Build();

        _gameRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.Game>()))
            .ReturnsAsync(expectedGame);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0m, result.BasePrice);
    }
}
