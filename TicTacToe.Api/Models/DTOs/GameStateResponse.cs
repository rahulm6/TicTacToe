using TicTacToe.Api.Models.Domain;
using TicTacToe.Api.Models.Enums;

namespace TicTacToe.Api.Models.DTOs;

public class GameStateResponse
{
    public Guid Id { get; set; }

    public GameMode Mode { get; set; }

    public string[][] Board { get; set; } = default!;

    public Player CurrentPlayer { get; set; }

    public GameStatus Status { get; set; }

    public Player? Winner { get; set; }

    public List<int> WinningCells { get; set; } = [];

    public List<Move> MoveHistory { get; set; } = [];
}