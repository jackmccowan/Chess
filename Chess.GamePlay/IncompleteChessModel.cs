namespace Chess.GamePlay
{
    public class IncompleteChessModel : IChessModel
    {
        /// ----------------------------------------------------------------------------------
        /// ----------- No need to edit this introductory code -------------------------------
        /// ----------- (scroll down to see code you need to edit) ---------------------------
        /// ----------------------------------------------------------------------------------

        public IncompleteChessModel() { }

        public bool IsPieceOwnedByPlayer(char piece, Player player)
        {
            GridCharacter[] allWhitePieces = {
                GridCharacter.WhiteQueen,
                GridCharacter.WhiteKing,
                GridCharacter.WhiteBishop,
                GridCharacter.WhiteRook,
                GridCharacter.WhiteKnight,
                GridCharacter.WhitePawn
            };
            GridCharacter[] allBlackPieces = {
                GridCharacter.BlackQueen,
                GridCharacter.BlackKing,
                GridCharacter.BlackBishop,
                GridCharacter.BlackRook,
                GridCharacter.BlackKnight,
                GridCharacter.BlackPawn
            };
            GridCharacter[] allowedPieces = player == Player.White ? allWhitePieces : allBlackPieces;
            return allowedPieces.Contains((GridCharacter)piece);
        }

        public void MovePiece(char[][] board, Move move)
        {
            char sourcePiece = board[move.fromRow][move.fromColumn];
            board[move.fromRow][move.fromColumn] = (char)GridCharacter.Empty;
            board[move.toRow][move.toColumn] = sourcePiece;
        }

        private bool IsIndexInRange(int index)
        {
            return index >= 0 && index < 8;
        }

        public bool IsMoveWithinBoard(Move move)
        {
            return IsIndexInRange(move.fromRow) && IsIndexInRange(move.fromColumn) && IsIndexInRange(move.toRow) && IsIndexInRange(move.toColumn);
        }

        public bool IsMoveFromOwnPiece(char[][] board, Move move, Player player)
        {
            char fromPiece = board[move.fromRow][move.fromColumn];
            return IsPieceOwnedByPlayer(fromPiece, player);
        }

        public bool IsMoveToValidTile(char[][] board, Move move, Player player)
        {
            char toPiece = board[move.toRow][move.toColumn];
            return !IsPieceOwnedByPlayer(toPiece, player);
        }

        public bool IsMoveLegal(char[][] board, Move move, Player turn)
        {
            return IsMoveWithinBoard(move)
                && IsMoveFromOwnPiece(board, move, turn)
                && IsMoveToValidTile(board, move, turn)
                && IsPieceMoveLegal(board, move, turn)
                && !IsMoveIntoCheck(board, move, turn);
        }

        private bool IsPieceMoveLegal(char[][] board, Move move, Player turn)
        {
            char fromPiece = board[move.fromRow][move.fromColumn];
            switch (fromPiece.ToString().ToLower())
            {
                case "k":
                    return IsValidMovementForKing(board, move, turn);
                case "n":
                    return IsValidMovementForKnight(board, move, turn);
                case "p":
                    return IsValidMovementForPawn(board, move, turn);
                case "r":
                    return IsValidMovementForRook(board, move, turn);
                case "b":
                    return IsValidMovementForBishop(board, move, turn);
                case "q":
                    return IsValidMovementForQueen(board, move, turn);
                default:
                    return true;
            }
        }

        public bool IsValidMovementForKing(char[][] board, Move move, Player player)
        {
            int rowDifference = move.fromRow - move.toRow;
            int columnDifference = move.fromColumn - move.toColumn;

            // A valid move must not start and end at the same place
            if (rowDifference == 0 && columnDifference == 0)
            {
                return false;
            }

            // A valid move must not move a distance of 2+ in either direction
            else if (rowDifference < -1 || rowDifference > 1 || columnDifference < -1 || columnDifference > 1)
            {
                return false;
            }

            // Everything else (within 1 square) is legal
            else return true;
        }


        /// ----------------------------------------------------------------------------------
        /// ----------- Edit from here for Part 1: Piece Logic -------------------------------
        /// ----------------------------------------------------------------------------------

        public bool IsValidMovementForKnight(char[][] board, Move move, Player player)
        {
            int rowDifference = move.fromRow - move.toRow;
            int columnDifference = move.fromColumn - move.toColumn;

            if (rowDifference == 0 && columnDifference == 0)
            {
                return false;
            }


            else if (rowDifference == 2 && columnDifference == 1 || columnDifference < -1)
            {
                return true;
            }
            else if (rowDifference == -2 && columnDifference == 1 || columnDifference == -1)
            {
                return true;
            }
            else if (columnDifference == 2 && rowDifference == -1 || rowDifference == 1)
            {
                return true;
            }
            else if (columnDifference == -2 && rowDifference == -1 || rowDifference == 1)
            {
                return true;
            }



            else return false;


           
        }

        public bool IsValidMovementForRook(char[][] board, Move move, Player player)
        {
            
            //rook can move vertically and horizontal, as long as there is nothing in the way. 
           
            int rowDifference = move.fromRow - move.toRow;
            int columnDifference = move.fromColumn - move.toColumn;

            if (rowDifference == 0 && columnDifference == 0)
            {
                return false;
            }

            else if ((rowDifference > 0 && columnDifference > 0 || columnDifference < 0) || (rowDifference < 0 && columnDifference < 0 || columnDifference > 0))
            {
                return false;
            }
            


            else if (move.fromRow < move.toRow) //upwards
            {
                for (int i = move.fromRow; i <= move.toRow; i++)
                {
                    if (board[i][move.fromColumn] != (char)GridCharacter.Empty)
                    {
                        return false;
                    }
                }
            }

            else if (move.toRow < move.fromRow)
            { //downwards
                for (int i = move.fromRow - 1; i > move.toRow; i--)
                {
                    if (board[i][move.fromColumn] != (char)GridCharacter.Empty)
                    {
                        return false;
                    }
                }
            }

            else if (move.fromColumn < move.toColumn) //Right Horizontal
            {
                for (int i = move.fromColumn; i <= move.toColumn; i++) {
                    if (board[i][move.fromRow] != (char)GridCharacter.Empty) {
                        return false;
                    }
                }
            }
            else if (move.toColumn < move.fromColumn)
            { //Left Horizontal
                for (int i = move.fromColumn - 1; i > move.toColumn; i--)
                {
                    if (board[move.fromRow][i] != (char)GridCharacter.Empty)
                    { //Changed board[i][move.fromColumn] to board[move.fromRow][i]
                        return false;
                    }
                }
            }

            return true; // todo implement this method. See IsValidMovementForKing for example.
        }

        public bool IsValidMovementForBishop(char[][] board, Move move, Player player)
        {

            return true; // todo implement this method. See IsValidMovementForKing for example.
        }

        public bool IsValidMovementForQueen(char[][] board, Move move, Player player)
        {
            return true; // todo implement this method. See IsValidMovementForKing for example.
        }//Test
       //Here
        public bool IsValidMovementForPawn(char[][] board, Move move, Player player)
        {

            //pawn can move up two only from its og position, then it can only move +1 vertical. It can also move +1 vertical and +-1 horizontal to attack. 

            //int rowDifference = move.fromRow - move.toRow;
            //int columnDifference = move.fromColumn - move.toColumn;

            //if (rowDifference == 0 && columnDifference == 0)
            //{
            //    return false;
            //}

            //else if ((move.fromRow != 2 || move.fromRow != 7) && rowDifference > 1)
            //{
            //    return false;
            //}

            return true; // todo implement this method. See IsValidMovementForKing for example.
        }

        /// ----------------------------------------------------------------------------------
        /// ----------- Edit from here for Part 2: End game logic ----------------------------
        /// ----------------------------------------------------------------------------------

        public bool IsInCheck(char[][] board, Player player)
        {
            return false; // todo implement this method.
        }

        public bool IsMoveIntoCheck(char[][] board, Move move, Player player)
        {
            return false; // todo implement this method.
        }

        public bool IsGameOver(char[][] board, Player player)
        {
            // todo update this method - the current implementation is incorrect and does not refer to the concept of "check".

            GridCharacter targetPiece = player == Player.White ? GridCharacter.WhiteKing : GridCharacter.BlackKing;

            // Search for the correct king (targetPiece).
            for (int i=0; i<board.Length; i++)
            {
                for (int j=0; j<board[i].Length; j++)
                {
                    if (board[i][j] == (char)targetPiece)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
