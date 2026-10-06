using TicTacToe.Api.Models.Enums;

namespace TicTacToe.Api.Models.Domain;

public class Game
{
    public Guid Id { get; set; }

    public GameMode Mode { get; set; }

    public string[][] Board { get; set; } =
    [
        ["","",""],
        ["","",""],
        ["","",""]
    ];

    public Player CurrentPlayer { get; set; }

    public GameStatus Status { get; set; }

    public Player? Winner { get; set; }

    public List<Move> MoveHistory { get; set; } = [];

    public List<int> WinningCells { get; set; } = [];
}