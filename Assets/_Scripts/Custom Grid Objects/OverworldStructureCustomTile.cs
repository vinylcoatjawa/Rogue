using UnityEngine;
/// <summary>
/// Custom tile to hold data about chunks of the overworld, used for structure generation
/// </summary>
public class OverworldStructureCustomTile
{
    public int _noOfPixels;
    public int _noOfGrassTiles;
    public float _noOfMountainTiles;
    public float _noOfWaterTiles;

    int _x, _z;
    Grid<OverworldStructureCustomTile> _grid; 
    public OverworldStructureCustomTile(Grid<OverworldStructureCustomTile> grid, int x, int z)
    {
        this._grid = grid;
        this._x = x;
        this._z = z;
    }
    /// <summary>
    /// Calculates the proportion of water on the map chunk
    /// </summary>
    /// <returns>Float with the water proportion</returns>
    public float GetWaterProportion()
    {
        return _noOfWaterTiles / (float)_noOfPixels;
    }
    /// <summary>
    /// Calculates the proportion of grass on the map chunk
    /// </summary>
    /// <returns>Float with the grass proportion</returns>
    public float GetGrassProportion()
    {
        return _noOfGrassTiles / (float)_noOfPixels;
    }
    /// <summary>
    /// Calculates the proportion of mountain on the map chunk
    /// </summary>
    /// <returns>Float with the mountain proportion</returns>
    public float GetMountainProportion()
    {
        return _noOfMountainTiles / (float)_noOfPixels;
    }
    /// <summary>
    /// Calculates the total amount of tiles in the map chunk
    /// </summary>
    /// <returns>Float with the total amount of tiles</returns>
    public float GetTotal()
    {
        return _noOfPixels;
    }

    public override string ToString()
    {
        string str = _x + ", " + _z;
        return str.ToString();
    }
}
