using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToeWpf;

public partial class MainWindow : Window
{
    private readonly char[] marks = new char[9];
    private readonly int[][] victoryLines =
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8],
        [0, 3, 6], [1, 4, 7], [2, 5, 8],
        [0, 4, 8], [2, 4, 6]
    ];
    private char activeMark;
    private int movesPlayed;
    private bool roundFinished;

    public MainWindow()
    {
        InitializeComponent();
        StartRound();
    }

    private void SquarePressed(object sender, RoutedEventArgs eventArgs)
    {
        if (roundFinished || sender is not Button pressedSquare)
            return;

        int squareIndex = int.Parse(pressedSquare.Tag.ToString()!);
        if (marks[squareIndex] != '\0')
            return;

        marks[squareIndex] = activeMark;
        movesPlayed++;
        pressedSquare.Content = activeMark;
        pressedSquare.Foreground = activeMark == 'X' ? Brushes.SteelBlue : Brushes.IndianRed;

        int[]? completedLine = FindCompletedLine();
        if (completedLine != null)
        {
            TurnLabel.Text = $"Игрок {activeMark} победил!";
            foreach (int winningSquare in completedLine)
                ((Button)PlayField.Children[winningSquare]).Background = Brushes.PaleGreen;
            FinishRound();
        }
        else if (movesPlayed == marks.Length)
        {
            TurnLabel.Text = "Ничья! Можно начать заново.";
            FinishRound();
        }
        else
        {
            activeMark = activeMark == 'X' ? 'O' : 'X';
            TurnLabel.Text = $"Сейчас ходит {activeMark}";
        }
    }

    private int[]? FindCompletedLine()
    {
        foreach (int[] line in victoryLines)
            if (line.All(squareIndex => marks[squareIndex] == activeMark))
                return line;

        return null;
    }

    private void FinishRound()
    {
        roundFinished = true;
        foreach (Button squareButton in PlayField.Children)
            squareButton.IsEnabled = false;
    }

    private void RestartPressed(object sender, RoutedEventArgs eventArgs)
    {
        StartRound();
    }

    private void StartRound()
    {
        Array.Clear(marks);
        activeMark = 'X';
        movesPlayed = 0;
        roundFinished = false;
        TurnLabel.Text = "Сейчас ходит X";

        foreach (Button squareButton in PlayField.Children)
        {
            squareButton.Content = "";
            squareButton.IsEnabled = true;
            squareButton.Background = Brushes.White;
            squareButton.Foreground = Brushes.SteelBlue;
        }
    }
}
