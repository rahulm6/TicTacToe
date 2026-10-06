using Microsoft.AspNetCore.Mvc;
using TicTacToe.Api.Interfaces;

namespace TicTacToe.Api.Controllers;

[ApiController]
[Route("api/scoreboard")]
public class ScoreboardController : ControllerBase
{
    private readonly IScoreboardService _scoreboardService;

    public ScoreboardController(IScoreboardService scoreboardService)
    {
        _scoreboardService = scoreboardService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_scoreboardService.GetScoreboard());
    }

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _scoreboardService.ResetScoreboard();

        return Ok(_scoreboardService.GetScoreboard());
    }
}