using System;
using UnityEngine;

    /// <summary>
    /// Grid class holding custom grid objects on a 2D grid 
    /// </summary>
    public class Grid<TGridObject>
    {
        // Default orientation X-Z axis with X being the width and Z being the height
        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private bool _allowDebug;
        private TGridObject[,] _gridArray;
        TextMesh[,] _debugTestArray;
        

        /// <summary>
        /// Grid constructor
        /// </summary>
        /// <param name="width">Number of columns in the grid</param>
        /// <param name="height"> Number of rows in the grid</param>
        /// <param name="cellSize">Sizelength of the quadratic gridpositions</param>
        /// <param name="originPosition">Coordinates of the bottom-leftmost gridcells position</param>
        /// <param name="createGridObject">Default grid object needed to initialize the Grid</param>
        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> createGridObject, bool allowDebug)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;
            this._allowDebug = allowDebug;


            Vector3 middleOffset = new Vector3(cellSize, 0, cellSize) * 0.5f;

        /* initializing array with default objects */
        _gridArray = new TGridObject[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    _gridArray[x, z] = createGridObject();
                }
            }
            _debugTestArray = new TextMesh[_width, _height];
            # region DEBUGARRAY
            if (_allowDebug)
            {
                GameObject debugGrid = new GameObject("debug grid");
                for (int x = 0; x < _width; x++)
                {
                    for (int z = 0; z < _height; z++)
                    {
                        _debugTestArray[x, z] = CreateWorldText(_gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + middleOffset, (int)_cellSize, Color.black, TextAnchor.MiddleCenter);
                        _debugTestArray[x, z].gameObject.transform.SetParent(debugGrid.transform);

                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.black, 100f);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.black, 100f);
                    }
                }

                Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.black, 100f);
            }

            #endregion
        }



        /// <summary>
        /// Returns the number of columns in the grid aka the 'x' value 
        /// </summary>
        /// <returns>Number of columns</returns>
        public int GetWitdth()
        {
            return _gridArray.GetLength(0);
        }
        /// <summary>
        /// Returns the number of rows in the grid aka the 'y' value 
        /// </summary>
        /// <returns>Number of rows</returns>
        public int GetHeight()
        {
            return _gridArray.GetLength(1);
        }
        /// <summary>
        /// Returns the 2D gridcells position in the 3D worldspace
        /// </summary>
        /// <param name="x">The gridcells 'x' coordinate</param>
        /// <param name="z">The gridcells 'z' coordinate</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int z, int y = 0) // convert from grid space to world space
        {
            return new Vector3(x, y, z) * _cellSize + _originPosition;
        }
        /// <summary>
        /// Returns the cellsize of the grid
        /// </summary>
        /// <returns>CellSize as float</returns>
        public float GetCellsize()
        {
            return _cellSize;
        }
        /// <summary>
        /// Sets the object stored on the grid position
        /// </summary>
        /// <param name="x">X coord of the cell</param>
        /// <param name="z">Z coord of the cell</param>
        /// <param name="value">The value to be set</param>
        public void SetGridObject(int x, int z, TGridObject value) // setting object on grid position
        {
            if (x >= 0 && z >= 0 && x < _width && z < _height)
            {
                _gridArray[x, z] = value;
                if (_debugTestArray[0,0] != null) { _debugTestArray[x, z].text = _gridArray[x, z]?.ToString(); }
                /*ugly way to see if allowDebug flag is true or not*/
            }
        }
    /// <summary>
    /// Reads and returns the object stored on grid position (x, z)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns>A custom grid object</returns>
    public TGridObject GetGridObject(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < _width && z < _height)
            {
                return _gridArray[x, z];
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
        /// <param name="z"></param>
        public void GetXY(Vector3 worldPosition, out int x, out int z) // convert from world space to grid space
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
        }
        /// <summary>
        /// Setting grid object using world space coordinates
        /// </summary>
        /// <param name="wordPosition"></param>
        /// <param name="value"></param>
        public void SetGridObject(Vector3 wordPosition, TGridObject value) // setting object in world space
        {
            int x, z;
            GetXY(wordPosition, out x, out z);
            SetGridObject(x, z, value);
        }
        /// <summary>
        /// Getting grid object based on world space coordinates
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public TGridObject GetGridObject(Vector3 worldPosition) // getting grid object from world position
        {
            int x, z;
            GetXY(worldPosition, out x, out z);
            if (x >= 0 && z >= 0 && x < _width && z < _height)
            {
                return GetGridObject(x, z);
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

            transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
      

            return textMesh;
        }
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment);
        }
        #endregion
    }

