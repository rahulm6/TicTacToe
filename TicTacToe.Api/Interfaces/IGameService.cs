using TicTacToe.Api.Models.Domain;
using TicTacToe.Api.Models.DTOs;

namespace TicTacToe.Api.Interfaces;

public interface IGameService
{
    Game CreateGame(CreateGameRequest request);

    Game GetGame(Guid gameId);

    Game MakeMove(Guid gameId, MoveRequest request);

    Game Undo(Guid gameId);

    Game ResetGame(Guid gameId);
}