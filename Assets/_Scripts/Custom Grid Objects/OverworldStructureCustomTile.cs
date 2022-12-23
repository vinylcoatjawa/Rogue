using UnityEngine;

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

    public float GetWaterPorportion()
    {
        return _noOfWaterTiles / (float)_noOfPixels;
    }
    public float GetGrassPorportion()
    {
        return _noOfGrassTiles / (float)_noOfPixels;
    }
    public float GetMountainPorportion()
    {
        return _noOfMountainTiles / (float)_noOfPixels;
    }
    public float GetTotal()
    {
        return _noOfPixels;
    }
}
