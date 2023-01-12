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
    int _dungeonFloorTileCount = 700;
    Vector3Int _gridOriginPosition;
    List<Vector3Int> _walkablbes;
    Grid<DungeonFloorTile> _floorTiles;
    CardinalDirection cardinalDirection;
    Noise _noise;
    Mesh _floorMesh;
    Mesh _wallMesh;
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
        _floorTiles = new Grid<DungeonFloorTile>( _width, _height, _cellSize, _gridOriginPosition, () => new DungeonFloorTile(_floorTiles, _width, _height), false);
        _thisDungeonInstance = SceneManager.GetActiveScene().name;
        _dungeonSeed = (int)OverworldMapData.GetType().GetField(_thisDungeonInstance + "_seed").GetValue(OverworldMapData);
    }

    
    void Start()
    {
        _floorMesh = new Mesh();
        _wallMesh = new Mesh();
        DLA();
        //DisplayWalkables();
        Debug.Log(_thisDungeonInstance);
        GeneratFloorMesh();
        //Debug.Log($"{_floorMesh.vertices.Count()}");
        AddWalls();
        //GeneratWallMesh();

    }

    

    void GeneratFloorMesh(){
        //if (GetComponent<MeshFilter>() == null) _floorMesh = gameObject.AddComponent<MeshFilter>().mesh = _floorMesh;
        GetComponent<MeshFilter>().mesh = _floorMesh; 
        Renderer rend = GetComponent<MeshRenderer>();
        
        MeshUtils.CreateEmptyMeshArrays(_dungeonFloorTileCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        //Vector3 quadSize = new Vector3(1,1) * _cellSize;
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;
        

        //Debug.Log($"vertices: {vertices.Count()} and uvs: {uvs.Count()} and triangles: {triangles.Count()}");
        
        int index = 0;
        
        for (int x = 0; x < _floorTiles.GetWitdth(); x++)
        {
            for (int z = 0; z < _floorTiles.GetHeight(); z++)
            {
                
                if (_floorTiles.GetGridObject(x, z).IsWalkable()){
                //int index = x * _floorTiles.GetHeight() + z;
                Vector3 pos = _floorTiles.GetWorldPosition(x, z);
                          
                //Debug.Log($"({x}, {z}) is walkable with index: {index} and at {pos}");

                int vIndex = index*4;
                int vIndex0 = vIndex;
                int vIndex1 = vIndex+1;
                int vIndex2 = vIndex+2;
                int vIndex3 = vIndex+3;


                vertices[vIndex0] = pos + Vector3.zero;
                vertices[vIndex1] = pos + new Vector3(0, 0, _cellSize);
                vertices[vIndex2] = pos + new Vector3(_cellSize, 0, _cellSize);
                vertices[vIndex3] = pos + new Vector3(_cellSize, 0, 0);

                //Debug.Log($"{vertices[vIndex0]} {vertices[vIndex1]} {vertices[vIndex2]} {vertices[vIndex3]}");
                

                uvs[vIndex0] = new Vector2(uv00.x, uv00.y) ;
                uvs[vIndex1] = new Vector2(uv00.x, uv11.y) ;
                uvs[vIndex2] = new Vector2(uv11.x, uv11.y) ;
                uvs[vIndex3] = new Vector2(uv11.x, uv00.y) ;


                int tIndex = index*6;

                triangles[tIndex+0] = vIndex0;
                triangles[tIndex+1] = vIndex1;
                triangles[tIndex+2] = vIndex2;
                
                triangles[tIndex+3] = vIndex0;
                triangles[tIndex+4] = vIndex2;
                triangles[tIndex+5] = vIndex3;
                
                
                index ++;

                }


            }
        }

        _floorMesh.vertices = vertices;
        _floorMesh.uv = uvs;
        _floorMesh.triangles = triangles;
        _floorMesh.RecalculateNormals();
        //Renderer rend = GetComponent<MeshRenderer>();
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
        rend.material = mat;
    }



    public void DLA(){
      
        SetMiddleWalkable();
        for (int i = 0; i < _dungeonFloorTileCount; i++)
        {
            cardinalDirection = PickAxis(i, _dungeonSeed);
            Vector3Int currentSelection = SelectRandomFromWalkableList(_walkablbes, i, _dungeonSeed);
            Expand(currentSelection, cardinalDirection);
        }
        
        
    }


private void AddWalls(){
    int wallQuadCount = 0;

        for (int x = 0; x < _floorTiles.GetWitdth(); x++)
        {
            for (int z = 0; z < _floorTiles.GetHeight(); z++)
            {
                DungeonFloorTile curr = _floorTiles.GetGridObject(x, z);
                
                if(_floorTiles.GetGridObject(x + 1, z) != null && !_floorTiles.GetGridObject(x + 1, z).IsWalkable()) { wallQuadCount++; curr._hasWallNorth = true; } 
                if(_floorTiles.GetGridObject(x, z + 1) != null && !_floorTiles.GetGridObject(x, z + 1).IsWalkable()) { wallQuadCount++; curr._hasWallEast = true; } 
                if(_floorTiles.GetGridObject(x - 1, z) != null && !_floorTiles.GetGridObject(x - 1, z).IsWalkable()) { wallQuadCount++; curr._hasWallSouth = true; } 
                if(_floorTiles.GetGridObject(x, z - 1) != null && !_floorTiles.GetGridObject(x, z - 1).IsWalkable()) { wallQuadCount++; curr._hasWallWest = true; } 
            }
        }
        Vector3[] vertices = _floorMesh.vertices;
        Vector2[] uvs = _floorMesh.uv;
        int[] triangles = _floorMesh.triangles;

        Array.Resize(ref vertices, (_dungeonFloorTileCount + wallQuadCount) * 4);
        Array.Resize(ref uvs, (_dungeonFloorTileCount + wallQuadCount) * 4);
        Array.Resize(ref triangles, (_dungeonFloorTileCount + wallQuadCount) * 6);


        GetComponent<MeshFilter>().mesh = _floorMesh; 
        Renderer rend = GetComponent<MeshRenderer>();

        int index = 0;


         

        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;

        for (int x = 0; x < 1/*_floorTiles.GetWitdth()*/; x++)
        {
            for (int z = 0; z < 1/*_floorTiles.GetHeight()*/; z++)
            {
                Debug.Log($" {x} and {z} has {_floorTiles.GetGridObject(x, z)._hasWallNorth}");
                if(_floorTiles.GetGridObject(x, z)._hasWallNorth){
                    Vector3 pos = _floorTiles.GetWorldPosition(x, z);
                          
                //Debug.Log($"({x}, {z}) is walkable with index: {index} and at {pos}");

                    int vIndex = index*4;
                    int vIndex0 = vIndex;
                    int vIndex1 = vIndex+1;
                    int vIndex2 = vIndex+2;
                    int vIndex3 = vIndex+3;


                    vertices[vIndex0] = pos + new Vector3(0, 0, _cellSize);//pos + Vector3.zero;
                    vertices[vIndex1] = pos + new Vector3(0, _cellSize * 2, _cellSize);//pos + new Vector3(0, 0, _cellSize);
                    vertices[vIndex2] = pos + new Vector3(_cellSize, _cellSize * 2, _cellSize);
                    vertices[vIndex3] = pos + new Vector3(_cellSize, 0, _cellSize);

                    //Debug.Log($"{vertices[vIndex0]} {vertices[vIndex1]} {vertices[vIndex2]} {vertices[vIndex3]}");
                    

                    uvs[vIndex0] = new Vector2(uv00.x, uv00.y) ;
                    uvs[vIndex1] = new Vector2(uv00.x, uv11.y) ;
                    uvs[vIndex2] = new Vector2(uv11.x, uv11.y) ;
                    uvs[vIndex3] = new Vector2(uv11.x, uv00.y) ;


                    int tIndex = index*6;

                    triangles[tIndex+0] = vIndex0;
                    triangles[tIndex+1] = vIndex1;
                    triangles[tIndex+2] = vIndex2;
                    
                    triangles[tIndex+3] = vIndex0;
                    triangles[tIndex+4] = vIndex2;
                    triangles[tIndex+5] = vIndex3;
                    
                    
                    index ++;
                }
            }

        _floorMesh.vertices = vertices;
        _floorMesh.uv = uvs;
        _floorMesh.triangles = triangles;
        _floorMesh.RecalculateNormals();
        //Renderer rend = GetComponent<MeshRenderer>();
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
        rend.material = mat;
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
                farthest = new Vector3Int(_walkablbes.Where(v => v.z == currentGridTile.z).Max(v => v.x), 0, currentGridTile.z); // finding the eastmost walkable
                choiceGridPos = new Vector3Int(farthest.x + 1, 0, farthest.z); // selecting the one east from it
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            case CardinalDirection.South:
                farthest =  new Vector3Int(currentGridTile.x, 0, _walkablbes.Where(v => v.x == currentGridTile.x).Min(v => v.z)); // finding the southmost walkable
                choiceGridPos = new Vector3Int(farthest.x, 0, farthest.z - 1); // selecting the one south from it
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            case CardinalDirection.West:
                farthest = new Vector3Int(_walkablbes.Where(v => v.z == currentGridTile.z).Min(v => v.x), 0, currentGridTile.z); // finding the westmost walkable
                choiceGridPos = new Vector3Int(farthest.x - 1, 0, farthest.z); // selecting the one west from it
                if(_floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z) != null) {
                _walkablbes.Add(choiceGridPos);
                _floorTiles.GetGridObject(choiceGridPos.x, choiceGridPos.z).SetTileWalkable();
                break;
                }
                else break;
            default: Debug.Log("Something went wrong with cardinal directions"); break;
        }
    }

    private void SetMiddleWalkable(){
        _floorTiles.GetGridObject(_middleCellX, _middleCellZ).SetTileWalkable();
        _walkablbes.Add(new Vector3Int(_middleCellX, 0, _middleCellZ));
    }

    private CardinalDirection PickAxis(int position, int seed)
    {
        int enumKey = _noise.IntNoiseInRange(0, 4, position, seed);
        return (CardinalDirection)enumKey;
    }


    

    private Vector3Int SelectRandomFromWalkableList(List<Vector3Int> walkables, int index, int seed){
        Vector3Int tile;
        
        int length = walkables.Count;
        int tileIndex = _noise.IntNoiseInRange(0, length, index, seed);
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
