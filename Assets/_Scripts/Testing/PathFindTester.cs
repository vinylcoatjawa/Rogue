using UnityEngine;

public class PathFindTester : MonoBehaviour
{
    int _width = 20, _height = 20, _cellSize = 10;
    Grid<DungeonFloorTile> _grid;
    

    public void Init(){
    _grid = new Grid<DungeonFloorTile>( _width, _height, _cellSize, Vector3.zero, 
            (Grid<DungeonFloorTile> grid, int x, int z) => new DungeonFloorTile(grid, x, z), true);
    }

    public void RunThrough(){
        GameObject Cubes = new GameObject("Cubes");
        for (int x = 0; x < _grid.GetWitdth(); x++){
            for (int z = 0; z < _grid.GetHeight(); z++)
            {
                Debug.Log($"we are on {x}, {z}");
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                
                go.transform.position = new Vector3(x * _cellSize + _cellSize / 2, 0, z * _cellSize + _cellSize / 2);
                go.transform.localScale = new Vector3(9, 0, 9);
                go.transform.SetParent(Cubes.transform);
            }
        }

    }

}
