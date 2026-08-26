string[] AllPieces = { "King", "Queen", "Rook", "Rook", "Knight", "Knight", "Bishop", "Bishop" };

for (int i = 0; i < 8; i++)
{
    Console.WriteLine(AllPieces[i]);
}

Random r = new Random();
r.Shuffle(AllPieces.AsSpan());

Console.WriteLine("\n");        

for (int i = 0; i < 8; i++)
{
    Console.WriteLine(AllPieces[i]);
}