using FFG;
using UnityEngine;


public static class GridExtensionHelpers
{
    /// <summary>
    /// Returns true if the cells and the dimensions of the two grid are the same
    /// </summary>
    public static bool IsSame(this FFG.Grid gridA, FFG.Grid gridB)
    {
        bool validDimensions = true;

        if (gridA != null && gridB != null)
        {
            validDimensions &= gridA.TotalCellCount == gridB.TotalCellCount;
            validDimensions &= gridA.GridOrigin == gridB.GridOrigin;
            validDimensions &= gridA.GridDimension == gridB.GridDimension;
            validDimensions &= gridA.CellDimension == gridB.CellDimension;
        }
        else return false;


        if (validDimensions == true)
        {
            if (gridA.CellArray != null && gridB.CellArray != null)
            {
                for (int v = 0; v < gridA.GridDimension.y; ++v)
                    for (int h = 0; h < gridA.GridDimension.x; ++h)
                        if (gridA.CellArray[h, v] != gridB.CellArray[h, v])
                            return false;
            }
            else return false;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns true if the cells and the dimensions of the two grid are the same
    /// </summary>
    public static bool IsSame(this FFG.Grid gridA, FFG.GridComponent gridB)
    {
        bool validDimensions = true;

        if (gridA != null && gridB != null)
        {
            validDimensions &= gridA.TotalCellCount == gridB.TotalCellCount;
            validDimensions &= gridA.GridOrigin == gridB.GridOrigin;
            validDimensions &= gridA.GridDimension == gridB.GridDimension;
            validDimensions &= gridA.CellDimension == gridB.CellDimension;
        }
        else return false;


        if (validDimensions == true)
        {
            if (gridA.CellArray != null && gridB.GetCellArray() != null)
            {
                for (int v = 0; v < gridA.GridDimension.y; ++v)
                    for (int h = 0; h < gridA.GridDimension.x; ++h)
                        if (gridA.CellArray[h, v] != gridB.GetCellArray()[h, v])
                            return false;
            }
            else return false;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns true if the cells and the dimensions of the two grid are the same
    /// </summary>
    public static bool IsSame(this FFG.GridComponent gridA, FFG.GridComponent gridB)
    {
        bool validDimensions = true;

        if (gridA != null && gridB != null)
        {
            validDimensions &= gridA.TotalCellCount == gridB.TotalCellCount;
            validDimensions &= gridA.GridOrigin == gridB.GridOrigin;
            validDimensions &= gridA.GridDimension == gridB.GridDimension;
            validDimensions &= gridA.CellDimension == gridB.CellDimension;
        }
        else return false;


        if (validDimensions == true)
        {
            if (gridA.GetCellArray() != null && gridB.GetCellArray() != null)
            {
                for (int v = 0; v < gridA.GridDimension.y; ++v)
                    for (int h = 0; h < gridA.GridDimension.x; ++h)
                        if (gridA.GetCellArray()[h, v] != gridB.GetCellArray()[h, v])
                            return false;
            }
            else return false;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns true if the cells and the dimensions of the two grid are the same
    /// </summary>
    public static bool IsSame(this FFG.GridComponent gridA, FFG.Grid gridB)
    {
        bool validDimensions = true;

        if (gridA != null && gridB != null)
        {
            validDimensions &= gridA.TotalCellCount == gridB.TotalCellCount;
            validDimensions &= gridA.GridOrigin == gridB.GridOrigin;
            validDimensions &= gridA.GridDimension == gridB.GridDimension;
            validDimensions &= gridA.CellDimension == gridB.CellDimension;
        }
        else return false;


        if (validDimensions == true)
        {
            if (gridA.GetCellArray() != null && gridB.CellArray != null)
            {
                for (int v = 0; v < gridA.GridDimension.y; ++v)
                    for (int h = 0; h < gridA.GridDimension.x; ++h)
                        if (gridA.GetCellArray()[h, v] != gridB.CellArray[h, v])
                            return false;
            }
            else return false;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns true if the value in the cell is same
    /// </summary>
    public static bool IsSame(this Cell cellA, Cell cellB)
    {
        bool result = true;

        result &= cellA.IsValid == cellB.IsValid;
        result &= cellA.Data == cellB.Data;

        return result;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGrid">Other grid</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.Grid thisGrid, FFG.Grid otherGrid, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGrid.GetGridBounds();
        Rect gridBBound = otherGrid.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGrid.GridDimension.x, thisGrid.GridDimension.y];
            var cellBounds = thisGrid.GetCellBoundsArray();
            var gridDimension = thisGrid.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGrid.CellArray[x, y].IsValid && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGrid.CellArray[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGridComponent">Other grid component</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.Grid thisGrid, FFG.GridComponent otherGridComponent, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGrid.GetGridBounds();
        Rect gridBBound = otherGridComponent.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGrid.GridDimension.x, thisGrid.GridDimension.y];
            var cellBounds = thisGrid.GetCellBoundsArray();
            var gridDimension = thisGrid.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGrid.CellArray[x, y].IsValid && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGrid.CellArray[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGridComponent">Other grid component</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.GridComponent thisGridComponent, FFG.GridComponent otherGridComponent, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGridComponent.GetGridBounds();
        Rect gridBBound = otherGridComponent.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGridComponent.GridDimension.x, thisGridComponent.GridDimension.y];
            var cellBounds = thisGridComponent.GetCellBoundsArray();
            var gridDimension = thisGridComponent.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGridComponent.GetCellArray()[x, y].IsValid && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGridComponent.GetCellArray()[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGrid">Other grid</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.GridComponent thisGridComponent, FFG.Grid otherGrid, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGridComponent.GetGridBounds();
        Rect gridBBound = otherGrid.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGridComponent.GridDimension.x, thisGridComponent.GridDimension.y];
            var cellBounds = thisGridComponent.GetCellBoundsArray();
            var gridDimension = thisGridComponent.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGridComponent.GetCellArray()[x, y].IsValid && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGridComponent.GetCellArray()[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }
}
