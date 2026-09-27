using Lab2_TicTacToe.Commands;
using Lab2_TicTacToe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab2_TicTacToe.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private string symbol = "";

        public int Row { get; }
        public int Column { get; }

        public string Symbol
        {
            get => symbol;
            set
            {
                symbol = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClickCommand { get; }

        public CellViewModel(int row, int column, ICommand clickCommand)
        {
            Row = row;
            Column = column;

            ClickCommand = new RelayCommand(
                _ => clickCommand.Execute(this));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private TicTacToeGame game;

        private bool gameOver;

        public ObservableCollection<CellViewModel> Cells { get; }

        public ICommand NewGameCommand { get; }

        public int Difficulty { get; set; } = 3;

        public string Status { get; private set; } = "Ваш хід";

        public MainViewModel()
        {
            game = new TicTacToeGame();

            gameOver = false;

            Cells = new ObservableCollection<CellViewModel>();

            for (int row = 0; row < TicTacToeGame.BoardSize; row++)
            {
                for (int column = 0; column < TicTacToeGame.BoardSize; column++)
                {
                    Cells.Add(
                        new CellViewModel(
                            row,
                            column,
                            new RelayCommand(MakeHumanMove)));
                }
            }

            NewGameCommand = new RelayCommand(_ => NewGame());

            UpdateCells();
        }

        private void MakeHumanMove(object? parameter)
        {
            if (gameOver)
                return;

            if (parameter is not CellViewModel cell)
                return;

            if (!game.MakeMove(cell.Row, cell.Column, CellState.X))
                return;

            UpdateCells();

            if (FinishGame())
                return;

            var computerMove = game.ChooseComputerMove(Difficulty);

            game.MakeMove(
                computerMove.Row,
                computerMove.Column,
                CellState.O);

            UpdateCells();

            FinishGame();
        }

        private bool FinishGame()
        {
            CellState winner = game.CheckWinner();

            if (winner == CellState.X)
            {
                Status = "Ви перемогли!";
                gameOver = true;
                OnPropertyChanged(nameof(Status));
                return true;
            }

            if (winner == CellState.O)
            {
                Status = "Комп'ютер переміг!";
                gameOver = true;
                OnPropertyChanged(nameof(Status));
                return true;
            }

            if (game.IsDraw())
            {
                Status = "Нічия!";
                gameOver = true;
                OnPropertyChanged(nameof(Status));
                return true;
            }

            Status = "Ваш хід";
            OnPropertyChanged(nameof(Status));

            return false;
        }

        private void NewGame()
        {
            game = new TicTacToeGame();
            gameOver = false;

            UpdateCells();

            Status = "Ваш хід";
            OnPropertyChanged(nameof(Status));
        }

        private void UpdateCells()
        {
            foreach (var cell in Cells)
            {
                cell.Symbol = game.Board[cell.Row, cell.Column] switch
                {
                    CellState.X => "X",
                    CellState.O => "O",
                    _ => ""
                };
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}