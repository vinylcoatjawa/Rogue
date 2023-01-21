using UnityEngine;

public class DungeonFloorTile
{
    int _width, _height, _walkableNeighbourCount = 0;
    bool _isWalkable;
    Grid<DungeonFloorTile> _grid;
    //public bool _hasWallNorth, _hasWallEast, _hasWallSouth, _hasWallWest;
    public DungeonFloorTile(Grid<DungeonFloorTile> grid, int width, int height){
        this._grid = grid;
        this._width = width;
        this._height = height;

    }

    public void SetTileWalkable(){
        _isWalkable = true;
    }
    public void SetTileUnWalkable(){
        _isWalkable = false;
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
     
}
