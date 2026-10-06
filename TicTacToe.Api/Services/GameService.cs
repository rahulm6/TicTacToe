using TicTacToe.Api.Interfaces;
using TicTacToe.Api.Models.Domain;
using TicTacToe.Api.Models.DTOs;
using TicTacToe.Api.Models.Enums;
using TicTacToe.Api.Repositories;

namespace TicTacToe.Api.Services;

public class GameService : IGameService
{
    private readonly GameRepository _repository;
    private readonly IScoreboardService _scoreboardService;

    public GameService(GameRepository repository, IScoreboardService scoreboardService)
    {
        _repository = repository;
        _scoreboardService = scoreboardService;
    }

    public Game CreateGame(CreateGameRequest request)
    {
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Mode = request.Mode,
            CurrentPlayer = Player.X,
            Status = GameStatus.InProgress
        };

        _repository.Add(game);

        return game;
    }

    public Game GetGame(Guid gameId)
    {
        var game = _repository.Get(gameId);

        if (game is null)
        {
            throw new Exception("Game not found.");
        }

        return game;
    }

    public Game MakeMove(Guid gameId, MoveRequest request)
    {
        var game = GetGame(gameId);

        if (game.Status != GameStatus.InProgress)
        {
            throw new Exception("Game already completed.");
        }

        if (request.Row < 0 || request.Row > 2)
        {
            throw new Exception("Invalid row.");
        }

        if (request.Column < 0 || request.Column > 2)
        {
            throw new Exception("Invalid column.");
        }

        if (request.Player != game.CurrentPlayer)
        {
            throw new Exception("Wrong player turn.");
        }

        if (!string.IsNullOrWhiteSpace(game.Board[request.Row][request.Column]))
        {
            throw new Exception("Cell already occupied.");
        }

        var symbol = GameRules.GetPlayerSymbol(request.Player);

        game.Board[request.Row][request.Column] = symbol;

        game.MoveHistory.Add(new Move
        {
            MoveNumber = game.MoveHistory.Count + 1,
            Player = request.Player,
            Row = request.Row,
            Column = request.Column
        });

        UpdateGameStatus(game, request.Player);

        if (game.Status == GameStatus.InProgress)
        {
            game.CurrentPlayer =
                game.CurrentPlayer == Player.X
                    ? Player.O
                    : Player.X;
        }

        if (
            game.Mode == GameMode.Computer &&
            game.Status == GameStatus.InProgress &&
            game.CurrentPlayer == Player.O)
        {
            MakeComputerMove(game);
        }

        _repository.Update(game);

        return game;
    }

    public Game Undo(Guid gameId)
    {
        var game = GetGame(gameId);

        if (game.Status != GameStatus.InProgress)
        {
            throw new Exception(
                "Undo is not allowed after game completion.");
        }

        if (!game.MoveHistory.Any())
        {
            throw new Exception("No moves to undo.");
        }

        if (game.Mode == GameMode.TwoPlayer)
        {
            UndoTwoPlayer(game);
        }
        else
        {
            UndoComputer(game);
        }

        _repository.Update(game);

        return game;
    }

    public Game ResetGame(Guid gameId)
    {
        var game = GetGame(gameId);

        game.Board =
        [
            ["","",""],
        ["","",""],
        ["","",""]
        ];

        game.CurrentPlayer = Player.X;
        game.Status = GameStatus.InProgress;
        game.Winner = null;

        game.MoveHistory.Clear();

        game.WinningCells.Clear();

        _repository.Update(game);

        return game;
    }

    private void UpdateGameStatus(
    Game game,
    Player player)
    {
        var symbol = GameRules.GetPlayerSymbol(player);

        if (GameRules.HasWinner(game.Board, symbol))
        {
            game.Status = GameStatus.Won;
            game.Winner = player;
            game.WinningCells = GameRules.GetWinningCells(game.Board, symbol);

            if (player == Player.X)
            {
                _scoreboardService.IncrementXWin();
            }
            else
            {
                _scoreboardService.IncrementOWin();
            }

            return;
        }

        if (GameRules.IsBoardFull(game.Board))
        {
            game.Status = GameStatus.Draw;

            _scoreboardService.IncrementDraw();
        }
    }

    private void RemoveLastMove(Game game)
    {
        var lastMove = game.MoveHistory.Last();

        game.Board[lastMove.Row][lastMove.Column] = "";

        game.MoveHistory.RemoveAt(game.MoveHistory.Count - 1);
    }

    private void UndoTwoPlayer(Game game)
    {
        var removedMove = game.MoveHistory.Last();

        RemoveLastMove(game);

        game.CurrentPlayer = removedMove.Player;

        game.Status = GameStatus.InProgress;
        game.Winner = null;

        game.WinningCells.Clear();
    }

    private void UndoComputer(Game game)
    {
        if (game.MoveHistory.Count < 2)
        {
            throw new Exception(
                "Not enough moves to undo.");
        }

        RemoveLastMove(game);

        RemoveLastMove(game);

        game.CurrentPlayer = Player.X;

        game.Status = GameStatus.InProgress;

        game.Winner = null;

        game.WinningCells.Clear();
    }

    private void MakeComputerMove(Game game)
    {
        var move = GetComputerMove(game.Board);

        game.Board[move.Row][move.Column] = "O";

        game.MoveHistory.Add(new Move
        {
            MoveNumber = game.MoveHistory.Count + 1,
            Player = Player.O,
            Row = move.Row,
            Column = move.Column
        });

        UpdateGameStatus(game, Player.O);

        if (game.Status == GameStatus.InProgress)
        {
            game.CurrentPlayer = Player.X;
        }
    }

    private (int Row, int Column) GetComputerMove(
    string[][] board)
    {
        var winningMove =
            GameRules.FindWinningMove(board, "O");

        if (winningMove.HasValue)
        {
            return winningMove.Value;
        }

        var blockingMove =
            GameRules.FindWinningMove(board, "X");

        if (blockingMove.HasValue)
        {
            return blockingMove.Value;
        }

        if (string.IsNullOrWhiteSpace(board[1][1]))
        {
            return (1, 1);
        }

        var corners = new List<(int, int)>
    {
        (0,0),
        (0,2),
        (2,0),
        (2,2)
    };

        foreach (var corner in corners)
        {
            if (string.IsNullOrWhiteSpace(
                    board[corner.Item1][corner.Item2]))
            {
                return corner;
            }
        }

        return GameRules.GetAvailableCells(board)
            .First();
    }
}