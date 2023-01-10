using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.InputSystem;
using NoiseUtils;

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
    Vector3Int _gridOriginPosition;
    Grid<DungeonFloorTile> _floorTiles;
    List<Vector3Int> _walkablbes;
    CardinalDirection cardinalDirection;
    Noise _noise;
    int _dungeonSeed;
    string _thisDungeonInstance;

    
    private void Awake()
    {
       
        _noise = new Noise();
        _middleCellX = _width / 2;
        _middleCellZ = _height / 2;
        _offset = _cellSize / 2;
        _gridOriginPosition = new Vector3Int( -_middleCellX  * _cellSize, 0, -_middleCellZ * _cellSize); // this is the world pos of the grids lower left corner so that 0,0 in world space ends up in the middle 
        _walkablbes = new List<Vector3Int>();
        _floorTiles = new Grid<DungeonFloorTile>( _width, _height, _cellSize, _gridOriginPosition, () => new DungeonFloorTile(_floorTiles, _width, _height), true);
        _thisDungeonInstance = SceneManager.GetActiveScene().name;
        _dungeonSeed = (int)OverworldMapData.GetType().GetField(_thisDungeonInstance + "_seed").GetValue(OverworldMapData);
    }

    
    void Start()
    {
        DLA();
        DisplayWalkables();
        Debug.Log(_thisDungeonInstance);
    }





    public void DLA(){
      
        SetMiddleWalkable();
        for (int i = 0; i < 700; i++)
        {
            Vector3Int currentSelection = SelectRandomFromWalkableList(_walkablbes, (uint)i, (uint)_dungeonSeed);
            cardinalDirection = PickAxis((uint)i, (uint)_dungeonSeed);
            Expand(currentSelection, cardinalDirection);
        }
        
        
    }

private void DisplayWalkables(){
    for (int x = 0; x < _floorTiles.GetWitdth(); x++)
        {
            for (int z = 0; z < _floorTiles.GetHeight(); z++)
            {
                if(_floorTiles.GetGridObject(x,z).IsWalkable()) {
                     Vector3 worldPos = _floorTiles.GetWorldPosition(x, z, 0) + new Vector3(_offset, 0, _offset);
                     GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                     cube.transform.position = worldPos;
                     cube.transform.localScale = new Vector3(10, 0, 10);
                }
            }
        }
}

private void Expand(Vector3Int currentGridTile, CardinalDirection direction)
    {
        
        Vector3Int farthest;
        Vector3Int choiceGridPos;
        switch (direction)
        {
            case CardinalDirection.North:
                farthest =  new Vector3Int(currentGridTile.x, 0, _walkablbes.Where(v => v.x == currentGridTile.x).Max(v => v.z)); // finding the northernmost walkable
                choiceGridPos = new Vector3Int(farthest.x, 0, farthest.z + 1); // selecting the one north from it         
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            case CardinalDirection.East:
                farthest = new Vector3Int(_walkablbes.Where(v => v.z == currentGridTile.z).Max(v => v.x), 0, currentGridTile.z);
                choiceGridPos = new Vector3Int(farthest.x + 1, 0, farthest.z);
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            case CardinalDirection.South:
                farthest =  new Vector3Int(currentGridTile.x, 0, _walkablbes.Where(v => v.x == currentGridTile.x).Min(v => v.z));
                choiceGridPos = new Vector3Int(farthest.x, 0, farthest.z - 1);
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            case CardinalDirection.West:
                farthest = new Vector3Int(_walkablbes.Where(v => v.z == currentGridTile.z).Min(v => v.x), 0, currentGridTile.z);
                choiceGridPos = new Vector3Int(farthest.x - 1, 0, farthest.z);
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            default: Debug.Log("Something went wrong with cardinal directions, we return (0,0,0)"); break;
        }
    }

    private void SetMiddleWalkable(){
        _floorTiles.GetGridObject(_middleCellX, _middleCellZ).SetTileWalkable();
        _walkablbes.Add(new Vector3Int(_middleCellX, 0, _middleCellZ));
    }

    private CardinalDirection PickAxis(uint position, uint seed)
    {
        int enumKey = (int)_noise.NoiseInRange(0, 4, position, seed);
        return (CardinalDirection)enumKey;
    }


    

    private Vector3Int SelectRandomFromWalkableList(List<Vector3Int> walkables, uint index, uint seed){
        Vector3Int tile;
        
        int length = walkables.Count;
        int tileIndex = (int)_noise.NoiseInRange(0, (uint)length, (uint)index, (uint)seed);
        //int index = Random.Range(0, length);
        tile = walkables[tileIndex];
        return tile;
    }
    public void TestingMouseInput(InputAction.CallbackContext context){
        if(context.performed){
            Debug.Log("clicked");
            DLA();
            
        }
        
    }

}
