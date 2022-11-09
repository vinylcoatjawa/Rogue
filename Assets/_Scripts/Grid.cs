using System;
using UnityEngine;

namespace Mapgrid
{
    /// <summary>
    /// Grid class holding custom grid object
    /// </summary>
    public class Grid<TGridObject>
    {
        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private TGridObject[,] gridArray;

        public static event Action<int, int, int> GridObjectSet;

        /// <summary>
        /// Grid constructor
        /// </summary>
        /// <param name="width">Number of columns in the grid</param>
        /// <param name="height"> Number of rows in the grid</param>
        /// <param name="cellSize">Sizelength of the quadratic gridpositions</param>
        /// <param name="originPosition">Coordinates of the bottom-leftmost gridcells position</param>
        /// <param name="createGridObject">Default grid object</param>
        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;


            Vector3 middleOffset = new Vector3(cellSize, cellSize, 0) * 0.5f;

            /* initializing array with default objects */
            gridArray = new TGridObject[width, height];
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject();
                }
            }

            



        }
        /// <summary>
        /// Returns the number of columns in the grid, the 'x' value 
        /// </summary>
        /// <returns>Number of columns</returns>
        public int GetWitdth()
        {
            return gridArray.GetLength(0);
        }
        /// <summary>
        /// Returns the number of rows in the grid, the 'y' value 
        /// </summary>
        /// <returns>Number of rows</returns>
        public int GetHeight()
        {
            return gridArray.GetLength(1);
        }
        /// <summary>
        /// Returns the 2D gridcells position in the 3D worldspace
        /// </summary>
        /// <param name="x">The gridcells 'x' coordinate</param>
        /// <param name="y">The gridcells 'y' coordinate</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y) // convert from grid space to world space
        {
            return new Vector3(x, y, 0) * cellSize + originPosition;
        }

        /// <summary>
        /// Returns the cellsize of the grid
        /// </summary>
        /// <returns>Cellsize as float</returns>
        public float GetCellsize()
        {
            return cellSize;
        }
        /// <summary>
        /// Sets the object stored on the grid position
        /// </summary>
        /// <param name="x">X coord of the cell</param>
        /// <param name="y">Y coord of the cell</param>
        /// <param name="value">The value to be set</param>
        public void SetGridObject(int x, int y, TGridObject value) // setting object on grid position
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                GridObjectSet.Invoke(x, y, value);
                //if (allowDebug) debugTestArray[x, y].text = gridArray[x, y]?.ToString();
            }
        }

    }
}
