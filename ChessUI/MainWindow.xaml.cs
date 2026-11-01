using ChessGame;
using ChessGame.enums;
using ChessGame.Moves;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] ImagePieces = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveStorage = new Dictionary<Position, Move>();  // stores all possible moves
        private Position selectedPosition = null;
        private GameState gameState;
        private ChessBot chessBot = new ChessBot();
        private Move lastBotMove = null;
        private PromotionMove promotionMove;
        public MainWindow()
        {
            InitializeComponent();
            InitialiseBoard();

            gameState = new GameState(PlayerColour.White, Board.InitialBoard());
            DrawBoard(gameState.Board);
        }
        private void InitialiseBoard() // creates the board to add the images
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image image = new Image();
                    ImagePieces[i, j] = image;
                    GridPiece.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[i, j] = highlight;
                    GridHighlights.Children.Add(highlight);
                }
            }
        }
        private void DrawBoard(Board board) // adds images to the board
        {
            for (int row = 0; row < 8; row++) // hides highlights for bot
            {
                for (int col = 0; col < 8; col++)
                {
                    highlights[row, col].Fill = Brushes.Transparent;
                }
            }
            for (int i = 0;i < 8;i++)
            {
                for (int j = 0;j < 8;j++)
                {
                    Piece piece = board[i,j];
                    ImagePieces[i, j].Source = LoadImages.GetImage(piece);
                }
            }
            if (lastBotMove != null) // highlights the last move played by the bot
            {
                highlights[lastBotMove.StartPos.row, lastBotMove.StartPos.column].Fill =
                    new SolidColorBrush(Color.FromArgb(119, 221, 119, 0));

                highlights[lastBotMove.EndPos.row, lastBotMove.EndPos.column].Fill =
                    new SolidColorBrush(Color.FromArgb(119, 221, 119, 0));
            }
        }

        private void GridBoard_MouseDown(object sender, MouseButtonEventArgs e) // checks for mouse input
        {
            // gets the square the click was made on

            Point point = e.GetPosition(GridBoard);

            double squareSize = GridBoard.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);

            Position pos = new Position(row, column);

            if (selectedPosition == null)
            {
                StartSelectedPos(pos); // runs if first input
            }
            else
            {
                EndSelectedPos(pos); // runs if second input (giving the move)
            }
        }

        private void StartSelectedPos(Position pos) // finds all the legal moves for the piece selected and highlights them
        {
            IEnumerable<Move> moves = gameState.LegalMoves(pos);

            if (moves.Any())
            {
                selectedPosition = pos;
                StoreMoves(moves);
                Showhighlight();
            }
        }

        private void EndSelectedPos(Position pos) // moves the piece selected to the selected square
        {
            selectedPosition = null;
            removeHighlight();

            CastleMove castleMove = null;

            foreach (Move move in moveStorage.Values)
            {
                if (move is CastleMove && ((CastleMove)move).RookStart == pos)
                {
                    castleMove = (CastleMove)move;
                    break;
                }
            }

            if (castleMove != null)
            {
                ShowMove(castleMove);
            }
            else if (moveStorage.TryGetValue(pos, out Move move))
            {
               ShowMove(move);
            }
        }
        /* private void ShowMove(Move move) // does the given move, and redraws the board to show the piece in the new position
        {
            if (move is PromotionMove promotion)
            {
                promotionMove = promotion;
                PromotionPanel.Visibility = Visibility.Visible;
                return;
            }

            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
        }
        */

        private void ShowMove(Move move) // same as above but temporary with chess bot
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);

            // Checks whether game has ended
            if (gameState.Winner != null)
            {
                ShowGameOver();
                return;
            }

            // makes bot play blacks turn
            if (gameState.CurrentTurn == PlayerColour.Black)
            {
                Move botMove = chessBot.FindBestMove(gameState, 3);

                if (botMove != null)
                {
                    lastBotMove = botMove;
                    gameState.MakeMove(botMove);
                    DrawBoard(gameState.Board);

                    if (gameState.Winner != null)
                    {
                        ShowGameOver();
                        return;
                    }
                }
            }
        }

        private void StoreMoves(IEnumerable<Move> moves) // stores every legal move in the storage
        {
            moveStorage.Clear();
            foreach (Move move in moves)
            {
                moveStorage[move.EndPos] = move;
            }
        }

        private void Showhighlight() // highlights all legal moves
        {
            System.Windows.Media.Color colour = System.Windows.Media.Color.FromRgb(224, 148, 247);

            // highlights normal moves
            foreach (Position end in moveStorage.Keys)
            {
                highlights[end.row, end.column].Fill = new System.Windows.Media.SolidColorBrush(colour);
            }

            // highlights rooks for castling
            foreach (CastleMove move in moveStorage.Values.OfType<CastleMove>())
            {
                highlights[move.RookStart.row, move.RookStart.column].Fill = new System.Windows.Media.SolidColorBrush(colour);
            }
        }

        private void removeHighlight() // removes the highlight from the squares
        {
            // unhighlights normal moves
            foreach (Position end in moveStorage.Keys)
            {
                highlights[end.row, end.column].Fill = System.Windows.Media.Brushes.Transparent;
            }

            // unhighlights rooks for castling
            foreach (CastleMove move in moveStorage.Values.OfType<CastleMove>())
            {
                highlights[move.RookStart.row, move.RookStart.column].Fill = System.Windows.Media.Brushes.Transparent;
            }

        }
        private void QueenPromotionButton(object sender, RoutedEventArgs e)
        {
            CompletePromotion(PieceTypes.Queen);
        }

        private void RookPromotionButton(object sender, RoutedEventArgs e)
        {
            CompletePromotion(PieceTypes.Rook);
        }

        private void BishopPromotionButton(object sender, RoutedEventArgs e)
        {
            CompletePromotion(PieceTypes.Bishop);
        }

        private void KnightPromotionButton(object sender, RoutedEventArgs e)
        {
            CompletePromotion(PieceTypes.Knight);
        }
        private void CompletePromotion(PieceTypes pieceType)
        {
            if (promotionMove == null)
            {
                return;
            }

            PromotionMove completedMove = new PromotionMove(promotionMove.StartPos, promotionMove.EndPos, pieceType);

            gameState.MakeMove(completedMove);
            promotionMove = null;
            PromotionPanel.Visibility = Visibility.Collapsed;

            DrawBoard(gameState.Board);
        }

        private void ShowGameOver()
        {
            if (gameState.Winner != null)
            {
                GameOver window = new GameOver(gameState.Winner);

                window.Owner = this;
                window.ShowDialog();
            }
        }

    }
}