using UnityEngine;
/// <summary>
/// Custom tile to hold data about chunks of the overworld, used for structure generation
/// </summary>
public class OverworldStructureCustomTile
{
    private int _noOfPixels;
    private int _noOfGrassTiles;
    private float _noOfMountainTiles;
    private float _noOfWaterTiles;
    public OverworldStructureCustomTile(int noOfPixels, int noOfGrassTiles, int noOfMountainTiles, int noOfWaterTiles)
    {
        this._noOfPixels = noOfPixels;
        this._noOfGrassTiles = noOfGrassTiles;
        this._noOfMountainTiles = noOfMountainTiles;
        this._noOfWaterTiles = noOfWaterTiles;
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
}
