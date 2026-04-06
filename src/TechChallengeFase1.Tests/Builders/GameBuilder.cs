using TechChallengeFase1.Domain.DTOs;
using TechChallengeFase1.Domain.Entities;

namespace TechChallengeFase1.Tests.Builders;

public class GameBuilder
{
    private int _gameId = 1;
    private string _title = "Test Game";
    private string? _description = null;
    private int _developerId = 1;
    private int _publisherId = 1;
    private DateTime _releaseDate = new DateTime(2024, 1, 1);
    private decimal _basePrice = 59.99m;
    private Guid _createdBy = Guid.NewGuid();

    public GameBuilder WithGameId(int gameId) { _gameId = gameId; return this; }
    public GameBuilder WithTitle(string title) { _title = title; return this; }
    public GameBuilder WithBasePrice(decimal price) { _basePrice = price; return this; }
    public GameBuilder WithCreatedBy(Guid createdBy) { _createdBy = createdBy; return this; }

    public Game Build()
    {
        var game = new Game(_title, _developerId, _publisherId, _releaseDate, _basePrice, _createdBy, _description);

        // Seta o GameId via reflection pois é gerado pelo banco
        typeof(Game).GetProperty(nameof(Game.GameId))!.SetValue(game, _gameId);

        return game;
    }

    public GameInputDto BuildDto() => new GameInputDto
    {
        Title = _title,
        Description = _description,
        DeveloperId = _developerId,
        PublisherId = _publisherId,
        ReleaseDate = _releaseDate,
        BasePrice = _basePrice,
        CreatedBy = _createdBy,
        IsActive = true
    };
}
