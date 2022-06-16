using System.Collections.Generic;
using UnityEngine;


namespace FFG
{
    public sealed class GridComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GridComponentDataContainer _gcData;

        private Grid _grid = null;

        #endregion Fields
        #region Properties

        public Vector2 GridOrigin => _gcData._gridOffset + (Vector2)transform.position;
        public Vector2Int GridDimension => _gcData._gridDimension;
        public Vector2 CellDimension => _gcData._cellDimension;
        public int TotalCellCount => _gcData._gridDimension.x * _gcData._gridDimension.y;

        #endregion Properties
        #region Public Methods

        /// <summary>
        /// Returns true if x and y is inside the grid
        /// </summary>
        /// <param name="x">X value of the grid</param>
        /// <param name="y">Y value of the grid</param>
        public bool IsInside(int x, int y) => _grid.IsInside(x, y);

        /// <summary>
        /// Returns true if the point is inside the grid
        /// </summary>
        /// <param name="worldPosition">Target world position</param>
        public bool IsInside(Vector3 worldPosition) => _grid.IsInside(worldPosition);

        /// <summary>
        /// Converts world position into grid sections
        /// </summary>
        /// <param name="worldPosition">Target position</param>
        /// <param name="x">Out parameter for x</param>
        /// <param name="y">Out parameter for y</param>
        public void GetXY(Vector3 worldPosition, out int x, out int y) => _grid.GetXY(worldPosition, out x, out y);

        /// <summary>
        /// Converts grid sections to world points
        /// </summary>
        /// <param name="x">Target x</param>
        /// <param name="y">Target y</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y) => _grid.GetWorldPosition(x, y);

        /// <summary>
        /// Returns the grid bounds
        /// </summary>
        public Rect GetGridBounds() => _grid.GetGridBounds();

        /// <summary>
        /// Returns the target cell's bounds
        /// </summary>
        /// <param name="x">Target x value</param>
        /// <param name="y">Target y value</param>
        public Rect GetCellBounds(int x, int y) => _grid.GetCellBounds(x, y);

        /// <summary>
        /// Returns the bounds of all cells
        /// </summary>
        public Rect[,] GetCellBoundsArray() => _grid.GetCellBoundsArray();

        /// <summary>
        /// Returns a list of all cell centers
        /// </summary>
        public Vector3[,] GetCellCenters() => _grid.GetCellCenters();

        /// <summary>
        /// Returns the center of a cell
        /// </summary>
        /// <param name="x">X value of a cell</param>
        /// <param name="y">Y value of a cell</param>
        /// <returns></returns>
        public Vector3 GetCellCenter(int x, int y) => _grid.GetCellCenter(x, y);

        /// <summary>
        /// Returns the center of a cell
        /// </summary>
        /// <param name="worldPosition">World position of the cell</param>
        /// <returns></returns>
        public Vector3 GetCellCenter(Vector3 worldPosition) => _grid.GetCellCenter(worldPosition);

        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public object GetValue(int x, int y) => _grid.GetValue(x, y);

        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public object GetValue(Vector2 worldPosition) => _grid.GetValue(worldPosition);

        /// <summary>
        /// Sets the value of the target grid section
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        /// <param name="value">Target value</param>
        public void SetValue(int x, int y, object value) => _grid.SetValue(x, y, value);

        /// <summary>
        /// Sets the value of the target grid sectio using world position
        /// </summary>
        /// <param name="worldPosition">World position</param>
        /// <param name="value">Target value</param>
        public void SetValue(Vector3 worldPosition, object value) => _grid.SetValue(worldPosition, value);

        /// <summary>
        /// Makes the cell valid
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void EnableCell(int x, int y) => _grid.EnableCell(x, y);

        /// <summary>
        /// Makes the cell valid
        /// </summary>
        /// <param name="worldPosition">World position</param>
        public void EnableCell(Vector2 worldPosition) => _grid.EnableCell(worldPosition);

        /// <summary>
        /// Makes the cell invalid
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void DisableCell(int x, int y) => _grid.DisableCell(x, y);

        /// <summary>
        /// Makes the cell invalid
        /// </summary>
        /// <param name="worldPosition">World position</param>
        public void DisableCell(Vector2 worldPosition) => _grid.DisableCell(worldPosition);

        /// <summary>
        /// Overrided
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _grid.ToString();

        /// <summary>
        /// Returns cell array
        /// </summary>
        public Cell[,] GetCellArray() => _grid.CellArray;

        #endregion
        #region Lifecycle methods

        private void Awake()
        {
            List<Vector2Int> invalidIndexList = new List<Vector2Int>();
            _grid = new Grid(GridOrigin, GridDimension.x, GridDimension.y, CellDimension);
            invalidIndexList.AddRange(_gcData._invalidCellArray);

            foreach (var index in invalidIndexList)
            {
                if (_grid.IsInside(index.x, index.y))
                    _grid.CellArray[index.x, index.y].IsValid = false;
                else
                    invalidIndexList.Remove(index);
            }

            _gcData._invalidCellArray = invalidIndexList.ToArray();
        }

        private void Update()
        {
            _grid.GridOrigin = GridOrigin;
        }

        private void OnDrawGizmos()
        {
            if (_gcData._shouldDrawGizmos == false)
                return;

            Gizmos.color = _gcData._gridLineColor;


            Vector2 corner = GridOrigin + new Vector2
                (
                    _gcData._gridDimension.x * _gcData._cellDimension.x,
                    _gcData._gridDimension.y * _gcData._cellDimension.y
                );

            // Horizontals
            Gizmos.DrawLine(GridOrigin, new Vector2(corner.x, GridOrigin.y));
            Gizmos.DrawLine(new Vector2(GridOrigin.x, corner.y), corner);
            // Verticals
            Gizmos.DrawLine(GridOrigin, new Vector2(GridOrigin.x, corner.y));
            Gizmos.DrawLine(new Vector2(corner.x, GridOrigin.y), corner);

            // Horizontal lines
            for (int h = 1; h < _gcData._gridDimension.y; ++h)
                Gizmos.DrawLine(GridOrigin + (Vector2.up * h * _gcData._cellDimension.y), new Vector2(corner.x, GridOrigin.y) + (Vector2.up * h * _gcData._cellDimension.y));

            // Vertical lines
            for (int w = 1; w < _gcData._gridDimension.x; ++w)
                Gizmos.DrawLine(GridOrigin + (Vector2.right * w * _gcData._cellDimension.x), new Vector2(GridOrigin.x, corner.y) + (Vector2.right * w * _gcData._cellDimension.x));


            Gizmos.color = _gcData._crossLineColor;

            foreach (var index in _gcData._invalidCellArray)
            {
                if (index.x >= 0 && index.x < _gcData._gridDimension.x && index.y >= 0 && index.y < _gcData._gridDimension.y)
                {
                    Vector2 cellBL = new Vector2(index.x * _gcData._cellDimension.x, index.y * _gcData._cellDimension.y) + GridOrigin;
                    Vector2 cellBR = new Vector2(cellBL.x + _gcData._cellDimension.x, cellBL.y);
                    Vector2 cellTL = new Vector2(cellBL.x, cellBL.y + _gcData._cellDimension.y);
                    Vector2 cellTR = new Vector2(cellBL.x + _gcData._cellDimension.x, cellBL.y + _gcData._cellDimension.y);

                    Gizmos.DrawLine(cellBL, cellTR);
                    Gizmos.DrawLine(cellBR, cellTL);
                }
            }
        }

        #endregion Lifecycle methods
        #region Nested types

        private enum OffsetType
        {
            Preset,
            Custom
        }

        private enum PresetTypes
        {
            TopRight,
            TopCenter,
            TopLeft,
            MiddleRight,
            MiddleCenter,
            MiddleLeft,
            BottomRight,
            BottomCenter,
            BottomLeft
        }

        [System.Serializable]
        private class GridComponentDataContainer
        {
            // Grid things
            [SerializeField]
            public Vector2Int _gridDimension = new Vector2Int();
            [SerializeField]
            public Vector2 _cellDimension = new Vector2();
            [SerializeField]
            public Vector2 _gridOffset = new Vector2();

            // Gizmos and editor things
            [SerializeField]
            public OffsetType _offsetType = OffsetType.Preset;
            [SerializeField]
            public PresetTypes _presetType = PresetTypes.BottomLeft;
            [SerializeField]
            public bool _shouldDrawGizmos = false;
            [SerializeField]
            public Color _gridLineColor;
            [SerializeField]
            public Color _crossLineColor;
            [SerializeField]
            public Vector2Int[] _invalidCellArray;
        }

        #endregion Nested types
    }
}