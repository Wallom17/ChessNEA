using ChessGame;
using ChessGame.enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOver : Window
    {
        public GameOver(Winner winner)
        {
            InitializeComponent();

            if (winner.How == GameEnd.Checkmate)
            {
                ResultText.Text = winner.Won + " wins by checkmate!";
            }
            else
            {
                ResultText.Text = "Draw by " + GetReason(winner.How) + ".";
            }
        }

        private string GetReason(GameEnd reason)
        {
            switch (reason)
            {
                case GameEnd.Stalemate:
                    return "stalemate";
                case GameEnd.InsufficientMaterial:
                    return "insufficient material";
                case GameEnd.FiftyMoves:
                    return "the 50-move rule";
                case GameEnd.ThreefoldRepetition:
                    return "threefold repetition";
                default:
                    return "unknown reason";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



    }
}
