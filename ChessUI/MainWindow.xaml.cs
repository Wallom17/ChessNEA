using ChessGame;
using ChessGame.enums;
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
            for (int i = 0;i < 8;i++)
            {
                for (int j = 0;j < 8;j++)
                {
                    Piece piece = board[i,j];
                    ImagePieces[i, j].Source = LoadImages.GetImage(piece);
                }
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

            if (moveStorage.TryGetValue(pos, out Move move))
            {
               ShowMove(move);
            }
        }
        private void ShowMove(Move move) // does the given move, and redraws the board to show the piece in the new position
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
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

            foreach (Position end in moveStorage.Keys)
            {
                highlights[end.row, end.column].Fill = new System.Windows.Media.SolidColorBrush(colour);
            }
        }

        private void removeHighlight() // removes the highlight from the squares
        {
            foreach (Position end in moveStorage.Keys)
            {
                highlights[end.row, end.column].Fill = System.Windows.Media.Brushes.Transparent;
            }   
        }
    }
}