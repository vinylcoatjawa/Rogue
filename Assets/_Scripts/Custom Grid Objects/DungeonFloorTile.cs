using System;
using UnityEngine;
/// <summary>
/// Tile type to use as the floor of the dungeon 
/// </summary>
public class DungeonFloorTile
{
    int _x, _z, _walkableNeighbourCount = 0;
    bool _isWalkable;
    Grid<DungeonFloorTile> _grid;
    //public bool _hasWallNorth, _hasWallEast, _hasWallSouth, _hasWallWest;
    public DungeonFloorTile(Grid<DungeonFloorTile> grid, int x, int z){
        this._grid = grid;
        this._x = x;
        this._z = z;

    }

    public void SetTileWalkable(){
        _isWalkable = true;
        _grid.TriggerGridObjectChanged(_x, _z);
    }
    public void SetTileUnWalkable(){
        _isWalkable = false;
        _grid.TriggerGridObjectChanged(_x, _z);
    }
    public bool IsWalkable(){
        return _isWalkable;
    }

    public void SetWalkableNeighbourCount(int count){
        _walkableNeighbourCount = count;
    }
    public int GetWalkableNeighbourCount(){
        return _walkableNeighbourCount;
    }

    public override string ToString()
    {
        return _isWalkable.ToString();
    }
     
}
