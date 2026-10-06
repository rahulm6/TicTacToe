using TicTacToe.Api.Models.Enums;

namespace TicTacToe.Api.Services;

public static class GameRules
{
    public static bool HasWinner(string[][] board, string symbol)
    {
        for (int row = 0; row < 3; row++)
        {
            if (board[row][0] == symbol &&
                board[row][1] == symbol &&
                board[row][2] == symbol)
            {
                return true;
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (board[0][col] == symbol &&
                board[1][col] == symbol &&
                board[2][col] == symbol)
            {
                return true;
            }
        }

        if (board[0][0] == symbol &&
            board[1][1] == symbol &&
            board[2][2] == symbol)
        {
            return true;
        }

        if (board[0][2] == symbol &&
            board[1][1] == symbol &&
            board[2][0] == symbol)
        {
            return true;
        }

        return false;
    }

    public static bool IsBoardFull(string[][] board)
    {
        return board
            .SelectMany(x => x)
            .All(x => !string.IsNullOrWhiteSpace(x));
    }

    public static string GetPlayerSymbol(Player player)
    {
        return player == Player.X ? "X" : "O";
    }

    public static List<int> GetWinningCells(string[][] board, string symbol)
    {
        for (int row = 0; row < 3; row++)
        {
            if (board[row][0] == symbol &&
                board[row][1] == symbol &&
                board[row][2] == symbol)
            {
                return [row * 3, row * 3 + 1, row * 3 + 2];
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (board[0][col] == symbol &&
                board[1][col] == symbol &&
                board[2][col] == symbol)
            {
                return [col, col + 3, col + 6];
            }
        }

        if (board[0][0] == symbol &&
            board[1][1] == symbol &&
            board[2][2] == symbol)
        {
            return [0, 4, 8];
        }

        if (board[0][2] == symbol &&
            board[1][1] == symbol &&
            board[2][0] == symbol)
        {
            return [2, 4, 6];
        }

        return [];
    }

    public static string[][] CloneBoard(string[][] board)
    {
        return board
            .Select(row => row.ToArray())
            .ToArray();
    }

    public static (int Row, int Column)? FindWinningMove(
    string[][] board,
    string symbol)
    {
        var available = GetAvailableCells(board);

        foreach (var cell in available)
        {
            var copy = CloneBoard(board);

            copy[cell.Row][cell.Column] = symbol;

            if (HasWinner(copy, symbol))
            {
                return cell;
            }
        }

        return null;
    }

    public static List<(int Row, int Column)> GetAvailableCells(string[][] board)
    {
        var cells = new List<(int, int)>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrWhiteSpace(board[row][col]))
                {
                    cells.Add((row, col));
                }
            }
        }

        return cells;
    }
}