using UnityEngine;

public class DungeonFloorTile
{
    int _width, _height;
    bool _isWalkable;
    Grid<DungeonFloorTile> _grid;
    public DungeonFloorTile(Grid<DungeonFloorTile> grid, int width, int height){
        this._grid = grid;
        this._width = width;
        this._height = height;

    }

    public void SetTileWalkable(){
        _isWalkable = true;
    }

    public bool AmIWalkable(){
        return _isWalkable;
    }

    
     
}
