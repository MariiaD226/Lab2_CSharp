using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_TicTacToe.Models
{
    public enum CellState
    {
        Empty,
        X,
        O
    }

    public class TicTacToeGame
    {
        public const int BoardSize = 3;

        public CellState[,] Board { get; private set; }

        public TicTacToeGame()
        {
            Board = new CellState[BoardSize, BoardSize];
        }

        public bool MakeMove(int row, int column, CellState player)
        {
            if (row < 0 || row >= BoardSize ||
                column < 0 || column >= BoardSize)
            {
                return false;
            }

            if (Board[row, column] != CellState.Empty)
            {
                return false;
            }

            Board[row, column] = player;

            return true;
        }

        public List<(int Row, int Column)> GetAvailableMoves()
        {
            var moves = new List<(int Row, int Column)>();

            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    if (Board[row, column] == CellState.Empty)
                    {
                        moves.Add((row, column));
                    }
                }
            }

            return moves;
        }

        public CellState CheckWinner()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                if (Board[row, 0] != CellState.Empty &&
                    Board[row, 0] == Board[row, 1] &&
                    Board[row, 1] == Board[row, 2])
                {
                    return Board[row, 0];
                }
            }

            for (int column = 0; column < BoardSize; column++)
            {
                if (Board[0, column] != CellState.Empty &&
                    Board[0, column] == Board[1, column] &&
                    Board[1, column] == Board[2, column])
                {
                    return Board[0, column];
                }
            }

            if (Board[0, 0] != CellState.Empty &&
                Board[0, 0] == Board[1, 1] &&
                Board[1, 1] == Board[2, 2])
            {
                return Board[0, 0];
            }

            if (Board[0, 2] != CellState.Empty &&
                Board[0, 2] == Board[1, 1] &&
                Board[1, 1] == Board[2, 0])
            {
                return Board[0, 2];
            }

            return CellState.Empty;
        }

        public bool IsDraw()
        {
            return CheckWinner() == CellState.Empty &&
                   GetAvailableMoves().Count == 0;
        }

        private readonly Random random = new Random();

        public (int Row, int Column) ChooseComputerMove(int level)
        {
            var availableMoves = GetAvailableMoves();

            if (availableMoves.Count == 0)
            {
                return (-1, -1);
            }

            if (level >= 3)
            {
                foreach (var move in availableMoves)
                {
                    Board[move.Row, move.Column] = CellState.O;

                    if (CheckWinner() == CellState.O)
                    {
                        Board[move.Row, move.Column] = CellState.Empty;
                        return move;
                    }

                    Board[move.Row, move.Column] = CellState.Empty;
                }
            }

            if (level >= 2)
            {
                foreach (var move in availableMoves)
                {
                    Board[move.Row, move.Column] = CellState.X;

                    if (CheckWinner() == CellState.X)
                    {
                        Board[move.Row, move.Column] = CellState.Empty;
                        return move;
                    }

                    Board[move.Row, move.Column] = CellState.Empty;
                }
            }

            return availableMoves[random.Next(availableMoves.Count)];
        }
    }
}