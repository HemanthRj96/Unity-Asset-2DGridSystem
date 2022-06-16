using UnityEngine;


namespace FFG
{
    /// <summary>
    /// Use this class to create grids of custom types
    /// </summary>
    [System.Serializable]
    public class Grid
    {
        #region Constructors

        public Grid()
        {
            GridOrigin = Vector2.zero;
            GridDimension = new Vector2Int(1, 1);
            CellDimension = new Vector2(1, 1);
            TotalCellCount = 1;
            CellArray = new Cell[1, 1];

            CellArray[0, 0] = new Cell();
            CellArray[0, 0].Index = Vector2Int.zero;
            CellArray[0, 0].IsValid = true;
        }

        public Grid(Vector3 origin, int horizontalCellCount, int verticalCellCount, Vector2 cellSize)
        {
            GridOrigin = origin;
            GridDimension = new Vector2Int(Mathf.Max(horizontalCellCount, 1), Mathf.Max(verticalCellCount, 1));
            CellDimension = new Vector2(Mathf.Max(cellSize.x, 0.01f), Mathf.Max(cellSize.y, 0.01f));
            TotalCellCount = GridDimension.x * GridDimension.y;
            CellArray = new Cell[horizontalCellCount, verticalCellCount];

            for (int x = 0; x < horizontalCellCount; x++)
                for (int y = 0; y < verticalCellCount; y++)
                {
                    CellArray[x, y] = new Cell();
                    CellArray[x, y].Index = new Vector2Int(x, y);
                    CellArray[x, y].IsValid = true;
                }
        }

        #endregion
        #region Properites

        public Vector2 GridOrigin { get; set; }
        public Vector2Int GridDimension { get; private set; }
        public Vector2 CellDimension { get; private set; }
        public int TotalCellCount { get; private set; }
        public Cell[,] CellArray { get; private set; }

        #endregion
        #region Public Methods

        /// <summary>
        /// Returns true if x and y is inside the grid
        /// </summary>
        /// <param name="x">X value of the grid</param>
        /// <param name="y">Y value of the grid</param>
        public bool IsInside(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < GridDimension.x && y < GridDimension.y)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the point is inside the grid
        /// </summary>
        /// <param name="worldPosition">Target world position</param>
        public bool IsInside(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return IsInside(x, y);
        }

        /// <summary>
        /// Converts world position into grid sections
        /// </summary>
        /// <param name="worldPosition">Target position</param>
        /// <param name="x">Out parameter for x</param>
        /// <param name="y">Out parameter for y</param>
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - (Vector3)GridOrigin).x / CellDimension.x);
            y = Mathf.FloorToInt((worldPosition - (Vector3)GridOrigin).y / CellDimension.y);
        }

        /// <summary>
        /// Converts grid sections to world points
        /// </summary>
        /// <param name="x">Target x</param>
        /// <param name="y">Target y</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector2(x * CellDimension.x, y * CellDimension.y) + GridOrigin;
        }

        /// <summary>
        /// Returns the grid bounds
        /// </summary>
        public Rect GetGridBounds()
        {
            return new Rect(GetWorldPosition(0, 0), new Vector2(CellDimension.x * GridDimension.x, CellDimension.y * GridDimension.y));
        }

        /// <summary>
        /// Returns the target cell's bounds
        /// </summary>
        /// <param name="x">Target x value</param>
        /// <param name="y">Target y value</param>
        /// <returns>Returns zero if invalid</returns>
        public Rect GetCellBounds(int x, int y)
        {
            if (IsInside(x, y))
                return new Rect(GetCellCenter(x, y), CellDimension);
            return Rect.zero;
        }

        /// <summary>
        /// Returns the bounds of all cells
        /// </summary>
        public Rect[,] GetCellBoundsArray()
        {
            Rect[,] bounds = new Rect[GridDimension.x, GridDimension.y];
            for (int x = 0; x < GridDimension.x; ++x)
                for (int y = 0; y < GridDimension.y; ++y)
                    bounds[x, y] = new Rect(GetWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2, CellDimension);
            return bounds;
        }

        /// <summary>
        /// Returns a list of all cell centers
        /// </summary>
        public Vector3[,] GetCellCenters()
        {
            Vector3[,] cellCenters = new Vector3[GridDimension.x, GridDimension.y];
            for (int y = 0; y < GridDimension.y; ++y)
                for (int x = 0; x < GridDimension.x; ++x)
                    cellCenters[x, y] = GetWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2;
            return cellCenters;
        }

        /// <summary>
        /// Returns the center of a cell
        /// </summary>
        /// <param name="x">X value of a cell</param>
        /// <param name="y">Y value of a cell</param>
        /// <returns>Returns positive infinity if the cell is not valid otherwise actual value</returns>
        public Vector3 GetCellCenter(int x, int y)
        {
            if (IsInside(x, y))
                return (GetWorldPosition(x, y) + new Vector3(CellDimension.x, CellDimension.y) / 2);
            return Vector3.positiveInfinity;
        }

        /// <summary>
        /// Returns the center of a cell
        /// </summary>
        /// <param name="worldPosition">World position of the cell</param>
        /// <returns>Returns positive infinity if the cell is not valid otherwise actual value</returns>
        public Vector3 GetCellCenter(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetCellCenter(x, y);
        }

        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public object GetValue(int x, int y)
        {
            if (IsInside(x, y))
                return CellArray[x, y].Data;
            return default;
        }

        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public object GetValue(Vector2 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetValue(x, y);
        }

        /// <summary>
        /// Sets the value of the target grid section
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        /// <param name="value">Target value</param>
        public void SetValue(int x, int y, object value)
        {
            if (x >= 0 && y >= 0 && x < GridDimension.y && y < GridDimension.x)
                CellArray[x, y].Data = value;
        }

        /// <summary>
        /// Sets the value of the target grid sectio using world position
        /// </summary>
        /// <param name="worldPosition">World position</param>
        /// <param name="value">Target value</param>
        public void SetValue(Vector3 worldPosition, object value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }

        /// <summary>
        /// Makes the cell valid
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void EnableCell(int x, int y)
        {
            if (IsInside(x, y))
                CellArray[x, y].IsValid = true;
        }

        /// <summary>
        /// Makes the cell valid
        /// </summary>
        /// <param name="worldPosition">World position</param>
        public void EnableCell(Vector2 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            EnableCell(x, y);
        }

        /// <summary>
        /// Makes the cell invalid
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void DisableCell(int x, int y)
        {
            if (IsInside(x, y))
                CellArray[x, y].IsValid = false;
        }

        /// <summary>
        /// Makes the cell invalid
        /// </summary>
        /// <param name="worldPosition">World position</param>
        public void DisableCell(Vector2 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            DisableCell(x, y);
        }

        /// <summary>
        /// Overrided
        /// </summary>
        public override string ToString()
        {
            string retValue = "";

            for (int v = 0; v < GridDimension.y; ++v)
            {
                for (int h = 0; h < GridDimension.x; ++h)
                    retValue += CellArray[h, v].ToString() + " ";
                retValue += "\n";
            }
            return retValue;
        }

        #endregion
    }
}
