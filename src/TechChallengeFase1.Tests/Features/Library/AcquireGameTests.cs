using Moq;
using TechChallengeFase1.Application.Services;
using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Exceptions;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Tests.Builders;

namespace TechChallengeFase1.Tests.Features.Library;

public class AcquireGameTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<ILibraryGameRepository> _libraryGameRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly LibraryService _sut;

    public AcquireGameTests()
    {
        _libraryRepositoryMock = new Mock<ILibraryRepository>();
        _libraryGameRepositoryMock = new Mock<ILibraryGameRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();

        _sut = new LibraryService(
            _libraryRepositoryMock.Object,
            _libraryGameRepositoryMock.Object,
            _gameRepositoryMock.Object);
    }

    [Fact]
    public async Task AcquireGameAsync_WithValidData_ReturnsLibraryGame()
    {
        // Arrange
        var dto = new LibraryGameInputDto { LibraryId = 1, GameId = 10 };
        var library = new LibraryBuilder().WithLibraryId(1).Build();
        var game = new GameBuilder().WithGameId(10).Build();
        var expectedLibraryGame = new TechChallengeFase1.Domain.Entities.LibraryGame(1, 10);

        _libraryRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(library);

        _gameRepositoryMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(game);

        _libraryGameRepositoryMock
            .Setup(r => r.ExistsAsync(1, 10))
            .ReturnsAsync(false);

        _libraryGameRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.LibraryGame>()))
            .ReturnsAsync(expectedLibraryGame);

        // Act
        var result = await _sut.AcquireGameAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.LibraryId);
        Assert.Equal(10, result.GameId);
        _libraryGameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.LibraryGame>()), Times.Once);
    }

    [Fact]
    public async Task AcquireGameAsync_WhenLibraryNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var dto = new LibraryGameInputDto { LibraryId = 99, GameId = 10 };

        _libraryRepositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((TechChallengeFase1.Domain.Entities.Library?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.AcquireGameAsync(dto));

        Assert.Contains("99", exception.Message);
        _libraryGameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.LibraryGame>()), Times.Never);
    }

    [Fact]
    public async Task AcquireGameAsync_WhenGameNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var dto = new LibraryGameInputDto { LibraryId = 1, GameId = 99 };
        var library = new LibraryBuilder().WithLibraryId(1).Build();

        _libraryRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(library);

        _gameRepositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((TechChallengeFase1.Domain.Entities.Game?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _sut.AcquireGameAsync(dto));

        Assert.Contains("99", exception.Message);
        _libraryGameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.LibraryGame>()), Times.Never);
    }

    [Fact]
    public async Task AcquireGameAsync_WhenGameAlreadyInLibrary_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new LibraryGameInputDto { LibraryId = 1, GameId = 10 };
        var library = new LibraryBuilder().WithLibraryId(1).Build();
        var game = new GameBuilder().WithGameId(10).WithTitle("Half-Life 3").Build();

        _libraryRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(library);

        _gameRepositoryMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(game);

        _libraryGameRepositoryMock
            .Setup(r => r.ExistsAsync(1, 10))
            .ReturnsAsync(true); // já existe

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.AcquireGameAsync(dto));

        Assert.Contains("Half-Life 3", exception.Message);
        _libraryGameRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TechChallengeFase1.Domain.Entities.LibraryGame>()), Times.Never);
    }

    [Fact]
    public async Task AcquireGameAsync_WithAcquiredFromOrderId_PreservesOrderReference()
    {
        // Arrange
        var dto = new LibraryGameInputDto { LibraryId = 1, GameId = 10, AcquiredFromOrderId = 42 };
        var library = new LibraryBuilder().WithLibraryId(1).Build();
        var game = new GameBuilder().WithGameId(10).Build();

        _libraryRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(library);
        _gameRepositoryMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(game);
        _libraryGameRepositoryMock.Setup(r => r.ExistsAsync(1, 10)).ReturnsAsync(false);
        _libraryGameRepositoryMock
            .Setup(r => r.AddAsync(It.Is<TechChallengeFase1.Domain.Entities.LibraryGame>(lg => lg.AcquiredFromOrderId == 42)))
            .ReturnsAsync(new TechChallengeFase1.Domain.Entities.LibraryGame(1, 10, acquiredFromOrderId: 42));

        // Act
        var result = await _sut.AcquireGameAsync(dto);

        // Assert
        Assert.Equal(42, result.AcquiredFromOrderId);
    }
}
