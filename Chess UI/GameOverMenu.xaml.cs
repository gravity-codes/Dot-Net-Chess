using ChessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess_UI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;
        
        public GameOverMenu(GameManager gameManager)
        {
            InitializeComponent();

            Result result = gameManager.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.EndReason, gameManager.Player);
        }

        private static string GetWinnerText(Player player)
        {
            return player switch
            {
                Player.White => "WHITE WINS",
                Player.Black => "Black WINS",
                _ => "DRAW"
            };
        }

        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "White",
                Player.Black => "Black",
                _ => ""
            };
        }

        private static string GetReasonText(EndReason endReason, Player player)
        {
            return endReason switch
            {
                EndReason.Stalemate => $"Stalemate: {PlayerString(player)} can't move!",
                EndReason.Checkmate => $"Checkmate: {PlayerString(player)} is mated!",
                EndReason.FiftyMoveRule => $"Fifty moves reached.",
                EndReason.InsufficientMaterial => $"Insufficient material: {PlayerString(player)} is out of pieces!",
                EndReason.ThreeFoldRepition => $"Repitition: {PlayerString(player)} repeated the same move 3 times.",
                _ => ""
            };
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
