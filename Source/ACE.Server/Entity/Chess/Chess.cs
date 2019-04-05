using System.Collections.Generic;
using System.Numerics;

using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    // ported from Anahera's code
    public class Chess
    {
        public const int BoardSize = 8;

        public const int NumColors = 2;

        public const int ChessWinnerStalemate = -1;
        public const int ChessWinnerEndGame = -2;     // finishes match with no winners

        public static readonly List<int> PieceWorth = new List<int>
        {
            0, 10, 50, 30, 30, 90, 900
        };

        /*public static readonly Dictionary<ChessColour, RookCastleFlag> RookFlags = new Dictionary<ChessColour, RookCastleFlag>()
        {
            { ChessColour.White, new RookCastleFlag(new Vector2(0, 0), ChessMoveFlag.QueenSideCastle) },
            { ChessColour.White, new RookCastleFlag(new Vector2(7, 0), ChessMoveFlag.KingSideCastle ) },
            { ChessColour.Black, new RookCastleFlag(new Vector2(0, 7), ChessMoveFlag.QueenSideCastle) },
            { ChessColour.Black, new RookCastleFlag(new Vector2(7, 7), ChessMoveFlag.KingSideCastle ) },
        };*/

        public static readonly Dictionary<ChessColor, List<RookCastleFlag>> RookFlags = new Dictionary<ChessColor, List<RookCastleFlag>>()
        {
            { ChessColor.White, new List<RookCastleFlag>() { new RookCastleFlag(new Vector2(0, 0), ChessMoveFlag.QueenSideCastle), new RookCastleFlag(new Vector2(7, 0), ChessMoveFlag.KingSideCastle)} },
            { ChessColor.Black, new List<RookCastleFlag>() { new RookCastleFlag(new Vector2(0, 7), ChessMoveFlag.QueenSideCastle), new RookCastleFlag(new Vector2(7, 7), ChessMoveFlag.KingSideCastle)} }
        };

        // GplpwdLphWtgt
        public static readonly Vector2[,] PawnOffsets = new Vector2[NumColors, 4]
        {
            { new Vector2(0,  1), new Vector2(0,  2), new Vector2( 1,  1), new Vector2(-1,  1) },
            { new Vector2(0, -1), new Vector2(0, -2), new Vector2(-1, -1), new Vector2( 1, -1) },
        };

        // determines valid moves for a particular piece
        /*public static readonly Dictionary<ChessPieceType, Vector2> PieceOffsets = new Dictionary<ChessPieceType, Vector2>()
        {
            { ChessPieceType.Rook,   new Vector2( 0,  1) }, { ChessPieceType.Rook,   new Vector2( 1,  0) }, { ChessPieceType.Rook,   new Vector2( 0, -1) }, { ChessPieceType.Rook,   new Vector2(-1,  0) },
            { ChessPieceType.Knight, new Vector2( 1,  2) }, { ChessPieceType.Knight, new Vector2( 2,  1) }, { ChessPieceType.Knight, new Vector2( 2, -1) }, { ChessPieceType.Knight, new Vector2( 1, -2) },
            { ChessPieceType.Knight, new Vector2(-1, -2) }, { ChessPieceType.Knight, new Vector2(-2, -1) }, { ChessPieceType.Knight, new Vector2(-2,  1) }, { ChessPieceType.Knight, new Vector2(-1,  2) },
            { ChessPieceType.Bishop, new Vector2( 1,  1) }, { ChessPieceType.Bishop, new Vector2( 1, -1) }, { ChessPieceType.Bishop, new Vector2(-1, -1) }, { ChessPieceType.Bishop, new Vector2(-1,  1) },
            { ChessPieceType.Queen,  new Vector2( 0,  1) }, { ChessPieceType.Queen,  new Vector2( 1,  1) }, { ChessPieceType.Queen,  new Vector2( 1,  0) }, { ChessPieceType.Queen,  new Vector2( 1, -1) },
            { ChessPieceType.Queen,  new Vector2( 0, -1) }, { ChessPieceType.Queen,  new Vector2(-1, -1) }, { ChessPieceType.Queen,  new Vector2(-1,  0) }, { ChessPieceType.Queen,  new Vector2(-1,  1) },
            { ChessPieceType.King,   new Vector2( 0,  1) }, { ChessPieceType.King,   new Vector2( 1,  1) }, { ChessPieceType.King,   new Vector2( 1,  0) }, { ChessPieceType.King,   new Vector2( 1, -1) },
            { ChessPieceType.King,   new Vector2( 0, -1) }, { ChessPieceType.King,   new Vector2(-1, -1) }, { ChessPieceType.King,   new Vector2(-1,  0) }, { ChessPieceType.King,   new Vector2(-1,  1) },
        };*/
        public static readonly Dictionary<ChessPieceType, List<Vector2>> PieceOffsets = new Dictionary<ChessPieceType, List<Vector2>>()
        {
            { ChessPieceType.Rook, new List<Vector2>() { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) } },
            { ChessPieceType.Knight, new List<Vector2>() { new Vector2(1, 2), new Vector2(2, 1), new Vector2(2, -1), new Vector2(1, -2), new Vector2(-1, -2), new Vector2(-2, -1), new Vector2(-2, 1), new Vector2(-1, 2) } },
            { ChessPieceType.Bishop, new List<Vector2>() { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) } },
            { ChessPieceType.Queen, new List<Vector2>() { new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1) } },
            { ChessPieceType.King, new List<Vector2>() { new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1) } },
        };

        // piece-square tables from Chess programming wiki
        public static float[,,] PieceSquareTable = new float[(int)ChessPieceType.Count, NumColors, BoardSize * BoardSize]
        {
	        // empty
	        {
                // white
                {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                },
                // black
                {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                }
            },
	        // pawn
	        {
		        // white
		        {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     5.0f,  5.0f,  5.0f,  5.0f,  5.0f,  5.0f,  5.0f,  5.0f,
                     1.0f,  1.0f,  2.0f,  3.0f,  3.0f,  2.0f,  1.0f,  1.0f,
                     0.5f,  0.5f,  1.0f,  2.5f,  2.5f,  1.0f,  0.5f,  0.5f,
                     0.0f,  0.0f,  0.0f,  2.0f,  2.0f,  0.0f,  0.0f,  0.0f,
                     0.5f, -0.5f, -1.0f,  0.0f,  0.0f, -1.0f, -0.5f,  0.5f,
                     0.5f,  1.0f,  1.0f, -2.0f, -2.0f,  1.0f,  1.0f,  0.5f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                },
		        // black
		        {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.5f,  1.0f,  1.0f, -2.0f, -2.0f,  1.0f,  1.0f,  0.5f,
                     0.5f, -0.5f, -1.0f,  0.0f,  0.0f, -1.0f, -0.5f,  0.5f,
                     0.0f,  0.0f,  0.0f,  2.0f,  2.0f,  0.0f,  0.0f,  0.0f,
                     0.5f,  0.5f,  1.0f,  2.5f,  2.5f,  1.0f,  0.5f,  0.5f,
                     1.0f,  1.0f,  2.0f,  3.0f,  3.0f,  2.0f,  1.0f,  1.0f,
                     5.0f,  5.0f,  5.0f,  5.0f,  5.0f,  5.0f,  5.0f,  5.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                }
            },
	        // rook
	        {
		        // white
		        {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.5f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                     0.0f,  0.0f,  0.0f,  0.5f,  0.5f,  0.0f,  0.0f,  0.0f
                },
		        // black
		        {
                     0.0f,  0.0f,  0.0f,  0.5f,  0.5f,  0.0f,  0.0f,  0.0f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                    -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f,
                     0.5f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  0.5f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                }
            },
	        // knight
	        {
		        // white
		        {
                    -5.0f, -4.0f, -3.0f, -3.0f, -3.0f, -3.0f, -4.0f, -5.0f,
                    -4.0f, -2.0f,  0.0f,  0.0f,  0.0f,  0.0f, -2.0f, -4.0f,
                    -3.0f,  0.0f,  1.0f,  1.5f,  1.5f,  1.0f,  0.0f, -3.0f,
                    -3.0f,  0.5f,  1.5f,  2.0f,  2.0f,  1.5f,  0.5f, -3.0f,
                    -3.0f,  0.0f,  1.5f,  2.0f,  2.0f,  1.5f,  0.0f, -3.0f,
                    -3.0f,  0.5f,  1.0f,  1.5f,  1.5f,  1.0f,  0.5f, -3.0f,
                    -4.0f, -2.0f,  0.0f,  0.5f,  0.5f,  0.0f, -2.0f, -4.0f,
                    -5.0f, -4.0f, -3.0f, -3.0f, -3.0f, -3.0f, -4.0f, -5.0f
                },
		        // black
                {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                }
            },
	        // bishop
	        {
		        // white
		        {
                    -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f,
                    -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f,
                    -1.0f,  0.0f,  0.5f,  1.0f,  1.0f,  0.5f,  0.0f, -1.0f,
                    -1.0f,  0.5f,  0.5f,  1.0f,  1.0f,  0.5f,  0.5f, -1.0f,
                    -1.0f,  0.0f,  1.0f,  1.0f,  1.0f,  1.0f,  0.0f, -1.0f,
                    -1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,
                    -1.0f,  0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.5f, -1.0f,
                    -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f
                },
		        // black
		        {
                    -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f,
                    -1.0f,  0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.5f, -1.0f,
                    -1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,
                    -1.0f,  0.0f,  1.0f,  1.0f,  1.0f,  1.0f,  0.0f, -1.0f,
                    -1.0f,  0.5f,  0.5f,  1.0f,  1.0f,  0.5f,  0.5f, -1.0f,
                    -1.0f,  0.0f,  0.5f,  1.0f,  1.0f,  0.5f,  0.0f, -1.0f,
                    -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f,
                    -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f
                }
            },
	        // queen
	        {
		        // white
		        {
                    -2.0f, -1.0f, -1.0f, -0.5f, -0.5f, -1.0f, -1.0f, -2.0f,
                    -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f,
                    -1.0f,  0.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -1.0f,
                    -0.5f,  0.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -0.5f,
                     0.0f,  0.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -0.5f,
                    -1.0f,  0.5f,  0.5f,  0.5f,  0.5f,  0.5f,  0.0f, -1.0f,
                    -1.0f,  0.0f,  0.5f,  0.0f,  0.0f,  0.0f,  0.0f, -1.0f,
                    -2.0f, -1.0f, -1.0f, -0.5f, -0.5f, -1.0f, -1.0f, -2.0f
                },
		        // black
                {
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,
                     0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f
                }
            },
	        // king
		        {
		        // white
		        {
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -2.0f, -3.0f, -3.0f, -4.0f, -4.0f, -3.0f, -3.0f, -2.0f,
                    -1.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -1.0f,
                     2.0f,  2.0f,  0.0f,  0.0f,  0.0f,  0.0f,  2.0f,  2.0f,
                     2.0f,  3.0f,  1.0f,  0.0f,  0.0f,  1.0f,  3.0f,  2.0f
                },
		        // black
		        {
                     2.0f,  3.0f,  1.0f,  0.0f,  0.0f,  1.0f,  3.0f,  2.0f,
                     2.0f,  2.0f,  0.0f,  0.0f,  0.0f,  0.0f,  2.0f,  2.0f,
                    -1.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -1.0f,
                    -2.0f, -3.0f, -3.0f, -4.0f, -4.0f, -3.0f, -3.0f, -2.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f,
                    -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f
                }
            }
        };

        public static ChessColor InverseColor(ChessColor color)
        {
            return color == ChessColor.White ? ChessColor.Black : ChessColor.White;
        }
    }
}
