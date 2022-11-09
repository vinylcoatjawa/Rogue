using System;
using UnityEngine;

    /// <summary>
    /// Grid class holding custom grid objects on a 2D grid 
    /// </summary>
    public class Grid<TGridObject>
    {
        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private TGridObject[,] gridArray;
        TextMesh[,] debugTestArray;


        public bool allowDebug = true;

        

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
            debugTestArray = new TextMesh[width, height];
            # region DEBUGARRAY
            if (allowDebug)
            {
                GameObject debugGrid = new GameObject("debug grid");
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTestArray[x, y] = CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + middleOffset, 20, Color.black, TextAnchor.MiddleCenter);
                        debugTestArray[x, y].gameObject.transform.SetParent(debugGrid.transform);

                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                    }
                }

                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);
            }

            #endregion
        }



        /// <summary>
        /// Returns the number of columns in the grid aka the 'x' value 
        /// </summary>
        /// <returns>Number of columns</returns>
        public int GetWitdth()
        {
            return gridArray.GetLength(0);
        }
        /// <summary>
        /// Returns the number of rows in the grid aka the 'y' value 
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
                if (allowDebug) debugTestArray[x, y].text = gridArray[x, y]?.ToString();
            }
        }
        /// <summary>
        /// Reads and returns the object stored on grid position (x, y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">A custom grid object</param>
        /// <returns></returns>
        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return default(TGridObject);
            }
        }
        /// <summary>
        /// Converts world space position to grid coordinates
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetXY(Vector3 worldPosition, out int x, out int y) // convert from world space to grid space
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }
        /// <summary>
        /// Setting grid object using world space coordinates
        /// </summary>
        /// <param name="wordPosition"></param>
        /// <param name="value"></param>
        public void SetGridObject(Vector3 wordPosition, TGridObject value) // setting object in world space
        {
            int x, y;
            GetXY(wordPosition, out x, out y);
            SetGridObject(x, y, value);
        }
        /// <summary>
        /// Setting grid object based on world space coordinates
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public TGridObject GetGridObject(Vector3 worldPosition) // getting grid object from world position
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return GetGridObject(x, y);
            }
            else
            {
                return default(TGridObject);
            }
        }
        #region UTILS


        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            return textMesh;
        }
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment);
        }
        #endregion
    }

