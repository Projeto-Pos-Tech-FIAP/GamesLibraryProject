using Moq;
using TechChallengeFase1.Application.Services;
using TechChallengeFase1.Domain.Interfaces;
using TechChallengeFase1.Tests.Builders;

namespace TechChallengeFase1.Tests.Features.Library;

public class GetUserLibraryTests
{
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<ILibraryGameRepository> _libraryGameRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly LibraryService _sut;

    public GetUserLibraryTests()
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
    public async Task GetByUserGuidAsync_WhenLibraryExists_ReturnsLibrary()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var expectedLibrary = new LibraryBuilder().WithUserGuid(userGuid).Build();

        _libraryRepositoryMock
            .Setup(r => r.GetByUserGuidAsync(userGuid))
            .ReturnsAsync(expectedLibrary);

        // Act
        var result = await _sut.GetByUserGuidAsync(userGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userGuid, result.UserGuid);
        _libraryRepositoryMock.Verify(r => r.GetByUserGuidAsync(userGuid), Times.Once);
    }

    [Fact]
    public async Task GetByUserGuidAsync_WhenUserHasNoLibrary_ReturnsNull()
    {
        // Arrange
        var userGuid = Guid.NewGuid();

        _libraryRepositoryMock
            .Setup(r => r.GetByUserGuidAsync(userGuid))
            .ReturnsAsync((TechChallengeFase1.Domain.Entities.Library?)null);

        // Act
        var result = await _sut.GetByUserGuidAsync(userGuid);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserGuidAsync_WhenLibraryHasGames_ReturnsLibraryWithGames()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var libraryWithGames = new LibraryBuilder()
            .WithUserGuid(userGuid)
            .WithGame(gameId: 10)
            .WithGame(gameId: 20)
            .Build();

        _libraryRepositoryMock
            .Setup(r => r.GetByUserGuidAsync(userGuid))
            .ReturnsAsync(libraryWithGames);

        // Act
        var result = await _sut.GetByUserGuidAsync(userGuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.LibraryGames.Count);
    }

    [Fact]
    public async Task GetByUserGuidAsync_AlwaysPassesCorrectGuidToRepository()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var differentGuid = Guid.NewGuid();

        _libraryRepositoryMock
            .Setup(r => r.GetByUserGuidAsync(userGuid))
            .ReturnsAsync(new LibraryBuilder().WithUserGuid(userGuid).Build());

        // Act
        await _sut.GetByUserGuidAsync(userGuid);

        // Assert — garante que nunca buscou com outro guid
        _libraryRepositoryMock.Verify(r => r.GetByUserGuidAsync(differentGuid), Times.Never);
        _libraryRepositoryMock.Verify(r => r.GetByUserGuidAsync(userGuid), Times.Once);
    }
}
