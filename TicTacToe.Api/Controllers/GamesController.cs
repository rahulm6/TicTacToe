using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Extensions;
using TicTacToe.Api.Interfaces;
using TicTacToe.Api.Models.DTOs;
using TicTacToe.Api.Services;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public IActionResult CreateGame(CreateGameRequest request)
    {
        var game = _gameService.CreateGame(request);

        return Ok(game);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetGame(Guid id)
    {
        var game = _gameService.GetGame(id);

        return Ok(game.ToResponse());
    }

    [HttpPost("{id:guid}/moves")]
    public IActionResult MakeMove(
    Guid id,
    MoveRequest request)
    {
        var game = _gameService.MakeMove(id, request);

        return Ok(game.ToResponse());
    }

    [HttpPost("{id:guid}/reset")]
    public IActionResult Reset(Guid id)
    {
        var game = _gameService.ResetGame(id);

        return Ok(game.ToResponse());
    }

    [HttpPost("{id:guid}/undo")]
    public IActionResult Undo(Guid id)
    {
        var game = _gameService.Undo(id);

        return Ok(game.ToResponse());
    }
}