using TicTacToe.Api.Models.Domain;
using TicTacToe.Api.Models.DTOs;

namespace TicTacToe.Api.Extensions;

public static class GameExtensions
{
    public static GameStateResponse ToResponse(
    this Game game)
    {
        return new GameStateResponse
        {
            Id = game.Id,
            Mode = game.Mode,
            Board = game.Board,
            CurrentPlayer = game.CurrentPlayer,
            Status = game.Status,
            Winner = game.Winner,
            WinningCells = game.WinningCells,
            MoveHistory = game.MoveHistory
        };
    }
}