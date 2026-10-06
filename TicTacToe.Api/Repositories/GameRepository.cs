using System.Collections.Concurrent;
using TicTacToe.Api.Models.Domain;

namespace TicTacToe.Api.Repositories;

public class GameRepository
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public void Add(Game game)
    {
        _games[game.Id] = game;
    }

    public Game? Get(Guid id)
    {
        return _games.GetValueOrDefault(id);
    }

    public void Update(Game game)
    {
        _games[game.Id] = game;
    }
}