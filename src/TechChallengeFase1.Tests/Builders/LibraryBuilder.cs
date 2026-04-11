using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Tests.Builders;

public class LibraryBuilder
{
    private int _libraryId = 1;
    private Guid _userGuid = Guid.NewGuid();
    private List<LibraryGame> _libraryGames = new();

    public LibraryBuilder WithLibraryId(int libraryId) { _libraryId = libraryId; return this; }
    public LibraryBuilder WithUserGuid(Guid userGuid) { _userGuid = userGuid; return this; }

    public LibraryBuilder WithGame(int gameId)
    {
        var libraryGame = new LibraryGame(_libraryId, gameId);
        _libraryGames.Add(libraryGame);
        return this;
    }

    public Library Build()
    {
        var library = new Library();

        typeof(Library).GetProperty(nameof(Library.LibraryId))!.SetValue(library, _libraryId);
        typeof(Library).GetProperty(nameof(Library.UserGuid))!.SetValue(library, _userGuid);
        typeof(Library).GetProperty(nameof(Library.IsActive))!.SetValue(library, true);
        typeof(Library).GetProperty(nameof(Library.CreatedAt))!.SetValue(library, DateTime.UtcNow);
        typeof(Library).GetProperty(nameof(Library.CreatedBy))!.SetValue(library, Guid.NewGuid());

        if (_libraryGames.Any())
            typeof(Library).GetProperty(nameof(Library.LibraryGames))!.SetValue(library, _libraryGames);

        return library;
    }
}
