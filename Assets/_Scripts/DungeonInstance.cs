using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.InputSystem;
public enum CardinalDirection { North = 0, East = 1, South = 2, West = 3 };

/// <summary>
/// Script on the spawned scene
/// </summary>
public class DungeonInstance : MonoBehaviour
{
    public OverworldMapData OverworldMapData;


    int _height = 51; // should be odd so we have a middle tile
    int _width = 51; // should be odd so we have a middle tile
    int _cellSize = 10;
    int _middleCellX;
    int _middleCellZ;
    int _offset;
    Vector3 _gridOriginPosition;
    string _currentDungeon;
    Grid<DungeonFloorTile> _floorTiles;
    List<Vector3Int> _walkablbes;
    CardinalDirection cardinalDirection;
    

    


    private void Awake()
    {
        _middleCellX = _width / 2 + 1;
        _middleCellZ = _height / 2 + 1;
        _offset = _cellSize / 2;
        _gridOriginPosition = new Vector3 ( -_middleCellX  * _cellSize + _offset, 0, -_middleCellZ * _cellSize + _offset); 
        _walkablbes = new List<Vector3Int>();
  
        _currentDungeon = SceneManager.GetActiveScene().name;
        _floorTiles = new Grid<DungeonFloorTile>( _width, _height, _cellSize, _gridOriginPosition, () => new DungeonFloorTile(_floorTiles, _width, _height), true);
        
    }

    public void TestingMouseInput(InputAction.CallbackContext context){
        if(context.performed){
            Vector3Int currentTile = SelectRandomFromList(_walkablbes);
            CardinalDirection direction = PickAxis();
            Vector3Int expandTo = Expand( currentTile, direction);
            //SetWalkable(_floorTiles, new Vector3Int(-1, 0, -1));
            Debug.Log($"From {currentTile} in {direction} so we add {expandTo}");
            _walkablbes.Add(expandTo);
            foreach (var item in _walkablbes)
            {
                Debug.Log(item);
            }
            SetWalkable(_floorTiles, expandTo);
            
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3Int(expandTo.x, 0, expandTo.z) * new Vector3Int(10, 0, 10);
            cube.transform.localScale = new Vector3(10, 0, 10);

        }
        
    }




    public void DLA(){
        // set middle walkable
        Vector3Int origin = Vector3Int.zero;
        SetWalkable(_floorTiles, origin);
        // add it to list
        _walkablbes.Add(origin);


        

        for (int i = 0; i < 500; i++)
        {
            
            Vector3Int currentTile = SelectRandomFromList(_walkablbes);
            //Debug.Log(_walkablbes.Count);
            CardinalDirection direction = PickAxis();
            //Debug.Log(direction);
            Vector3Int expandTo = Expand( currentTile, direction);

            //Debug.Log($"From {currentTile} in {direction} so we add {expandTo}");
            SetWalkable(_floorTiles, expandTo);
            _walkablbes.Add(expandTo);
            
            
        }
        
        foreach (var item in _walkablbes)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3Int(item.x, 0, item.z) * new Vector3Int(10, 0, 10);
            cube.transform.localScale = new Vector3(10, 0, 10);
        }

        for (int x = 0; x < _floorTiles.GetWitdth(); x++)
        {
            for (int z = 0; z < _floorTiles.GetHeight(); z++)
            {
                Vector3 pos = _floorTiles.GetWorldPosition(x, z);
                if(_floorTiles.GetGridObject(x,z).AmIWalkable()) Debug.Log($"({pos.x}, {pos.z}) is  walkabale");
            }
        }
        
        // select random tile from list
        // select random cardinal direction
        // find furthermost walkable in that direction
        // set next tile walkable in that direction
    }

    private Vector3Int Expand(Vector3Int currentTile, CardinalDirection direction)
    {
        Vector3Int choice;
        Vector3Int farthest;
        int fixedX = currentTile.x;
        int fixedZ = currentTile.z;
        switch (direction)
        {
            case CardinalDirection.North:
                farthest =  _walkablbes.Where(v => v.x == fixedX).ToList().OrderByDescending(v => v.z).First(); // finding the northernmost walkable
                choice = new Vector3Int(farthest.x, 0, farthest.z + 1); // selecting the one north from it         
                return choice;
            case CardinalDirection.East:
                farthest =  _walkablbes.Where(v => v.z == fixedZ).ToList().OrderByDescending(v => v.x).First();
                choice = new Vector3Int(farthest.x + 1, 0, farthest.z);
                return choice;
            case CardinalDirection.South:
                farthest =  _walkablbes.Where(v => v.x == fixedX).ToList().OrderBy(v => v.z).First();
                choice = new Vector3Int(farthest.x, 0, farthest.z - 1);
                return choice;
            case CardinalDirection.West:
                farthest =  _walkablbes.Where(v => v.z == fixedZ).ToList().OrderBy(v => v.x).First();
                choice = new Vector3Int(farthest.x - 1, 0, farthest.z);
                return choice;
            default: Debug.Log("Something went wrong with cardinal directions, we return (0,0,0)"); return Vector3Int.zero;
        }
    }

    private CardinalDirection PickAxis()
    {
        
        int enumKey = Random.Range(0,4);
        return (CardinalDirection)enumKey;
         
    }

    void Start()
    {
        DLA();
        

        
        /*
        
        Vector3Int origin = Vector3Int.zero;
        SetWalkable(_floorTiles, origin);
        
        _walkablbes.Add(origin);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3Int(0, 0, 0) * new Vector3Int(10, 0, 10);
        cube.transform.localScale = new Vector3(10, 0, 10);
        */
    }

    private void SetWalkable(Grid<DungeonFloorTile> grid, int x, int z){
        DungeonFloorTile tile = grid.GetGridObject(x, z);
        tile.SetTileWalkable();
    }
    private void SetWalkable(Grid<DungeonFloorTile> grid, Vector3Int position){
        int x, z;
        grid.GetXY(position, out x, out z);
        DungeonFloorTile tile = grid.GetGridObject(x, z);
        tile.SetTileWalkable();
    }

    private Vector3Int SelectRandomFromList(List<Vector3Int> walkables){
        Vector3Int tile;
        
        int length = walkables.Count;
        int index = Random.Range(0, length);
        tile = walkables[index];
        return tile;
    }

}
