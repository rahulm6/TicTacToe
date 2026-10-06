using TicTacToe.Api.Models.Enums;

namespace TicTacToe.Api.Models.DTOs;

public class MoveRequest
{
    public Player Player { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }
}