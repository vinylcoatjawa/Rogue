public class PathFindingTile{
    
    Grid<PathFindingTile> _grid;
    int _x, _z;

    public int GCost, HCost, FCost;

    public PathFindingTile CameFrom;

    public PathFindingTile(int x, int z){
        this._x = x;
        this._z = z;
    }

    public override string ToString()
    {
        return _x + ", " + _z;
    }
}