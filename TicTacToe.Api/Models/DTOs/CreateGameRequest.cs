using TicTacToe.Api.Models.Enums;

namespace TicTacToe.Api.Models.DTOs;

public class CreateGameRequest
{
    public GameMode Mode { get; set; }
}