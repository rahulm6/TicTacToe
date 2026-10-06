using FluentAssertions;
using TicTacToe.Api.Models.DTOs;
using TicTacToe.Api.Models.Enums;
using TicTacToe.Api.Repositories;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Tests;

public class GameServiceTests
{
    private readonly ScoreboardService _scoreboard;
    private readonly GameRepository _repository;
    private readonly GameService _service;

    public GameServiceTests()
    {
        _scoreboard = new ScoreboardService();
        _repository = new GameRepository();

        _service = new GameService(
            _repository,
            _scoreboard);
    }

    private Guid CreateGame(
        GameMode mode = GameMode.TwoPlayer)
    {
        var game = _service.CreateGame(
            new CreateGameRequest
            {
                Mode = mode
            });

        return game.Id;
    }

    [Fact]
    public void Valid_Move_Should_Update_Board()
    {
        var id = CreateGame();

        var result = _service.MakeMove(
            id,
            new MoveRequest
            {
                Player = Player.X,
                Row = 0,
                Column = 0
            });

        result.Board[0][0]
            .Should()
            .Be("X");
    }

    [Fact]
    public void Move_On_Occupied_Cell_Should_Throw()
    {
        var id = CreateGame();

        _service.MakeMove(id,
            new MoveRequest
            {
                Player = Player.X,
                Row = 0,
                Column = 0
            });

        Action action = () =>
            _service.MakeMove(id,
                new MoveRequest
                {
                    Player = Player.O,
                    Row = 0,
                    Column = 0
                });

        action.Should()
            .Throw<Exception>();
    }

    [Fact]
    public void Wrong_Player_Should_Throw()
    {
        var id = CreateGame();

        Action action = () =>
            _service.MakeMove(
                id,
                new MoveRequest
                {
                    Player = Player.O,
                    Row = 0,
                    Column = 0
                });

        action.Should()
            .Throw<Exception>();
    }

    [Fact]
    public void Turn_Should_Switch()
    {
        var id = CreateGame();

        var game =
            _service.MakeMove(
                id,
                new MoveRequest
                {
                    Player = Player.X,
                    Row = 0,
                    Column = 0
                });

        game.CurrentPlayer
            .Should()
            .Be(Player.O);
    }

    [Fact]
    public void Row_Win_Should_Be_Detected()
    {
        var id = CreateGame();

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 0 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 1 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 1 });

        var game =
            _service.MakeMove(id, new()
            {
                Player = Player.X,
                Row = 0,
                Column = 2
            });

        game.Status.Should()
            .Be(GameStatus.Won);

        game.Winner.Should()
            .Be(Player.X);
    }

    [Fact]
    public void Column_Win_Should_Be_Detected()
    {
        var id = CreateGame();

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 0, Column = 1 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 1, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 1 });

        var game =
            _service.MakeMove(id, new()
            {
                Player = Player.X,
                Row = 2,
                Column = 0
            });

        game.Status
            .Should()
            .Be(GameStatus.Won);
    }

    [Fact]
    public void Diagonal_Win_Should_Be_Detected()
    {
        var id = CreateGame();

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 0, Column = 1 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 1, Column = 1 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 0, Column = 2 });

        var game =
            _service.MakeMove(id, new()
            {
                Player = Player.X,
                Row = 2,
                Column = 2
            });

        game.Status
            .Should()
            .Be(GameStatus.Won);
    }

    [Fact]
    public void Draw_Should_Be_Detected()
    {
        var id = CreateGame();

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 0, Column = 1 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 2 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 1 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 1, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 2 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 2, Column = 1 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 2, Column = 0 });

        var game =
            _service.MakeMove(id, new()
            {
                Player = Player.X,
                Row = 2,
                Column = 2
            });

        game.Status
            .Should()
            .Be(GameStatus.Draw);
    }

    [Fact]
    public void Reset_Should_Clear_Board()
    {
        var id = CreateGame();

        _service.MakeMove(id,
            new MoveRequest
            {
                Player = Player.X,
                Row = 0,
                Column = 0
            });

        var game = _service.ResetGame(id);

        game.Board
            .SelectMany(x => x)
            .Should()
            .OnlyContain(x => x == "");

        game.MoveHistory
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void Undo_TwoPlayer_Should_Remove_Last_Move()
    {
        var id = CreateGame();

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 0 });

        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 1 });

        var game = _service.Undo(id);

        game.Board[1][1]
            .Should()
            .Be("");

        game.CurrentPlayer
            .Should()
            .Be(Player.O);
    }

    [Fact]
    public void Undo_Computer_Mode_Should_Remove_Two_Moves()
    {
        var id = CreateGame(
            GameMode.Computer);

        _service.MakeMove(id,
            new MoveRequest
            {
                Player = Player.X,
                Row = 0,
                Column = 0
            });

        var game = _service.Undo(id);

        game.MoveHistory.Count
            .Should()
            .Be(0);

        game.CurrentPlayer
            .Should()
            .Be(Player.X);
    }

    [Fact]
    public void Scoreboard_Should_Update()
    {
        var id = CreateGame();

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 0 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 0 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 1 });
        _service.MakeMove(id, new() { Player = Player.O, Row = 1, Column = 1 });

        _service.MakeMove(id, new() { Player = Player.X, Row = 0, Column = 2 });

        _scoreboard.GetScoreboard()
            .XWins
            .Should()
            .Be(1);
    }

    [Fact]
    public void Computer_Should_Take_Center()
    {
        var id =
            CreateGame(GameMode.Computer);

        var game =
            _service.MakeMove(
                id,
                new MoveRequest
                {
                    Player = Player.X,
                    Row = 0,
                    Column = 0
                });

        game.Board[1][1]
            .Should()
            .Be("O");
    }
}