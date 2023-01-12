using UnityEngine;

public class DungeonFloorTile
{
    int _width, _height;
    bool _isWalkable;
    Grid<DungeonFloorTile> _grid;
    public bool _hasWallNorth, _hasWallEast, _hasWallSouth, _hasWallWest;
    public DungeonFloorTile(Grid<DungeonFloorTile> grid, int width, int height){
        this._grid = grid;
        this._width = width;
        this._height = height;

    }

    public void SetTileWalkable(){
        _isWalkable = true;
    }

    public bool IsWalkable(){
        return _isWalkable;
    }

    
     
}
