using System;
using UnityEngine;
/// <summary>
/// Tile type to use as the floor of the dungeon 
/// </summary>
public class DungeonFloorTile
{

    public int GCost, HCost, FCost, GridX, GridZ;
    public DungeonFloorTile cameFromTile;
    /* this is for a possible smoothing algorithm */
    int _walkableNeighbourCount = 0;
    bool _isWalkable;
    Grid<DungeonFloorTile> _grid;
    public DungeonFloorTile(Grid<DungeonFloorTile> grid, int x, int z){
        this._grid = grid;
        this.GridX = x;
        this.GridZ = z;

    }
    /// <summary>
    /// Sets the floortile to walkable
    /// </summary>
    public void SetTileWalkable(){
        _isWalkable = true;
        //_grid.TriggerGridObjectChanged(X, Z);
    }
    /// <summary>
    /// Sets the floortile unwalkable
    /// </summary>
    public void SetTileUnWalkable(){
        _isWalkable = false;
        //_grid.TriggerGridObjectChanged(X, Z);
    }
    /// <summary>
    /// Returns whether the floortile is walkable
    /// </summary>
    /// <returns>Boolean about the walkability of the tile</returns>
    public bool IsWalkable(){
        return _isWalkable;
    }

    /// <summary>
    /// Counts the walkable tiles in the immediate vincinity of this tile may be needed for smoothing not used yet
    /// </summary>
    /// <param name="count">The number to which the neighbourcount is to be set</param>
    public void SetWalkableNeighbourCount(int count){
        _walkableNeighbourCount = count;
    }
    /// <summary>
    /// Returns the number of walakble tiles in the immediate vincinity of this tile
    /// </summary>
    /// <returns></returns>
    public int GetWalkableNeighbourCount(){
        return _walkableNeighbourCount;
    }
    /// <summary>
    /// Sets the fcost of this tile
    /// </summary>
    public void CalculateFCost(){
        FCost = GCost + HCost;
    }
    public override string ToString()
    {
        string str = "F: " + FCost.ToString() + "\n G: " + GCost.ToString() + "\n H: " + HCost.ToString();
        return str;
    }
     
}
