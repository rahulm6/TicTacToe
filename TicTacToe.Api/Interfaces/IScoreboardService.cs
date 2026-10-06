using TicTacToe.Api.Models.Domain;

namespace TicTacToe.Api.Interfaces;

public interface IScoreboardService
{
    Scoreboard GetScoreboard();

    void ResetScoreboard();

    void IncrementXWin();

    void IncrementOWin();

    void IncrementDraw();
}