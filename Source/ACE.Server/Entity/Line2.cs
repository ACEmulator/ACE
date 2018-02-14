using System.Numerics;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Represents a 2D line
    /// </summary>
    public class Line2
    {
        /// <summary>
        /// The start point
        /// </summary>
        public Vector2 Start;

        /// <summary>
        /// The end point
        /// </summary>
        public Vector2 End;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Line2()
        {
        }

        /// <summary>
        /// Constructs a line from 2 points
        /// </summary>
        public Line2(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Constructs a line from 2 points
        /// </summary>
        public Line2(float startX, float startY, float endX, float endY)
        {
            Start = new Vector2(startX, startY);
            End = new Vector2(endX, endY);
        }

        /// <summary>
        /// Constructs a 2D line from 3D points, dropping the Z-component
        /// </summary>
        public Line2(Vector3 start, Vector3 end)
        {
            Start = new Vector2(start.X, start.Y);
            End = new Vector2(end.X, end.Y);
        }

        /// <summary>
        /// Returns the determinant of a line at point
        /// </summary>
        public static float Determinant(Line2 line, float x, float y)
        {
            return (line.End.X - line.Start.X) * (y - line.Start.Y) - (line.End.Y - line.Start.Y) * (x - line.Start.X);
        }

        public static float Determinant(Line2 line, Vector2 point)
        {
            return Determinant(line, point.X, point.Y);
        }

        public float Determinant(Vector2 point)
        {
            return Determinant(this, point);
        }

        /// <summary>
        /// Returns TRUE if point resides on a line
        /// </summary>
        public static bool Collinear(Line2 line, Vector2 point)
        {
            return Determinant(line, point) == 0;
        }

        public static bool Collinear(Line2 line, float x, float y)
        {
            return Determinant(line, x, y) == 0;
        }

        public bool Collinear(float x, float y)
        {
            return Collinear(this, x, y);
        }

        public bool Collinear(Vector2 point)
        {
            return Collinear(this, point);
        }

        /// <summary>
        /// Returns TRUE if point is on left side of line
        /// </summary>
        /// <remarks>
        /// If we draw a horizontal line from left to right,
        /// The left side is considered to be the top.
        /// </remarks>
        public static bool LeftSide(Line2 line, Vector2 point)
        {
            return Determinant(line, point) < 0;
        }

        public static bool LeftSide(Line2 line, float x, float y)
        {
            return Determinant(line, x, y) < 0;
        }

        public bool LeftSide(Vector2 point)
        {
            return LeftSide(this, point);
        }

        public bool LeftSide(float x, float y)
        {
            return LeftSide(this, x, y);
        }

        /// <summary>
        /// Returns TRUE if point is on right side of line
        /// </summary>
        /// <remarks>
        /// If we draw a horizontal line from left to right,
        /// The right side is considered to be the bottom.
        /// </remarks>
        public static bool RightSide(Line2 line, Vector2 point)
        {
            return Determinant(line, point) > 0;
        }

        public static bool RightSide(Line2 line, float x, float y)
        {
            return Determinant(line, x, y) > 0;
        }

        public bool RightSide(Vector2 point)
        {
            return RightSide(this, point);
        }

        public bool RightSide(float x, float y)
        {
            return RightSide(this, x, y);
        }
    }
}
