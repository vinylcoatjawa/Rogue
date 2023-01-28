using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;
using NoiseUtils;
using System;

public enum CardinalDirection { North = 0, East = 1, South = 2, West = 3 };

/// <summary>
/// Script on the spawned scene handling the dungeon instance
/// </summary>
public class DungeonInstance : MonoBehaviour
{
    public OverworldMapData OverworldMapData;
    public DungeonFloorGridData dungeonFloorGridData;
    public GameObject PlayerPrefab;

    int _height = 51; // should be odd so we have a middle tile
    int _width = 51; // should be odd so we have a middle tile
    int _cellSize = 10;
    int _middleCellX;
    int _middleCellZ;
    int _offset;
    int _dungeonFloorTileCount = 1000;
    Vector3Int _gridOriginPosition;
    List<Vector3Int> _walkablbes;
    Grid<DungeonFloorTile> _floorTiles;
    Grid<GameObject> _movementGrid;
    CardinalDirection cardinalDirection;
    Noise _noise;
    Mesh _floorMesh;
    int _dungeonSeed;
    string _thisDungeonInstance;
    MeshCollider _floorMeshCollider;
    PlayerBaseState _playerState;
    int _floorMeshBitMask = 1 << 6;
    int _x, _z;
    Mesh _movementIndicator;
    PlayerStateManager _playerStateManager;
    [SerializeField] private Vector3Event OnMouseoverMovementTile;
    

    

    void Awake(){
        InputActions inputActions = new InputActions();
        inputActions.InputForTesting.Enable();
        inputActions.InputForTesting.Leftclick.performed += RecordTile;
        Init();
        Instantiate(PlayerPrefab, new Vector3(_cellSize / 2, 1, _cellSize / 2), Quaternion.identity);   
    }


    void Start(){
        DLA();      
        GeneratFloorMesh();
        _floorMeshCollider = gameObject.AddComponent<MeshCollider>();
        _floorMeshCollider.sharedMesh = _floorMesh;
        gameObject.tag = "FloorMesh";
        
    }
    void Update(){
        switch (_playerState)
        {
            case PlayerIdleState:
                break;
            case PlayerPlanMoveState:
            GetGridCoords();
            OnMouseoverMovementTile.Raise( _floorTiles.GetWorldPosition(_x, _z)); // sends the world position of the tile which we hover over        
                break;
            default:
                Debug.Log("default");
                break;
        }
    }    
    /// <summary>
    /// Generates a mesh by running through the grid and spawning a quad where the dungeon floor tile is tagged walkable
    /// </summary>
    void GeneratFloorMesh(){
        
        GetComponent<MeshFilter>().mesh = _floorMesh; 
        Renderer rend = GetComponent<MeshRenderer>();
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;
        
        MeshUtils.CreateEmptyMeshArrays(_dungeonFloorTileCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

        int index = 0;
        
        for (int x = 0; x < _floorTiles.GetWitdth(); x++){
            for (int z = 0; z < _floorTiles.GetHeight(); z++){
                
                if (_floorTiles.GetGridObject(x, z).IsWalkable()){

                    Vector3 pos = _floorTiles.GetWorldPosition(x, z);

                    int vIndex = index * 4;
                    int tIndex = index * 6;

                    int vIndex0 = vIndex;
                    int vIndex1 = vIndex+1;
                    int vIndex2 = vIndex+2;
                    int vIndex3 = vIndex+3;

                    vertices[vIndex0] = pos + Vector3.zero;
                    vertices[vIndex1] = pos + new Vector3(0, 0, _cellSize);
                    vertices[vIndex2] = pos + new Vector3(_cellSize, 0, _cellSize);
                    vertices[vIndex3] = pos + new Vector3(_cellSize, 0, 0);

                    uvs[vIndex0] = new Vector2(uv00.x, uv00.y) ;
                    uvs[vIndex1] = new Vector2(uv00.x, uv11.y) ;
                    uvs[vIndex2] = new Vector2(uv11.x, uv11.y) ;
                    uvs[vIndex3] = new Vector2(uv11.x, uv00.y) ;

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
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
        rend.material = mat;
        
    }
    /// <summary>
    /// Performs rudimentary DLA generation on a grid of possible floortiles
    /// </summary>
    void DLA(){
        SetMiddleWalkable();
        for (int i = 0; i < _dungeonFloorTileCount; i++)
        {
            cardinalDirection = PickAxis(i, _dungeonSeed);
            Vector3Int currentSelection = SelectRandomFromWalkableList(_walkablbes, i, _dungeonSeed);
            Expand(currentSelection, cardinalDirection);
        }
    }
    /// <summary>
    /// Adds a new floor tile to the already existing floorgrid
    /// </summary>
    /// <param name="currentGridTile">The worldpos of a randomly selected tile</param>
    /// <param name="direction">A cardinal direction in which we would like to add a new tile</param>
    void Expand(Vector3Int currentGridTile, CardinalDirection direction){
            
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
    /// <summary>
    /// Sets the middle grid cell walkable
    /// </summary>
    void SetMiddleWalkable(){
        _floorTiles.GetGridObject(_middleCellX, _middleCellZ).SetTileWalkable();
        _walkablbes.Add(new Vector3Int(_middleCellX, 0, _middleCellZ));
    }
    /// <summary>
    /// Selects a random cardinal direction based on a noise function
    /// </summary>
    /// <param name="position">The place in the noise function</param>
    /// <param name="seed">The seed of the noise function</param>
    /// <returns>An enum cardinal direction</returns>
    CardinalDirection PickAxis(int position, int seed){
        int enumKey = _noise.IntNoiseInRange(0, 4, position, seed);
        return (CardinalDirection)enumKey;
    }
    /// <summary>
    /// Initializes some values 
    /// </summary>
    void Init(){
        _noise = new Noise();
        _floorMesh = new Mesh();
        _middleCellX = _width / 2;
        _middleCellZ = _height / 2;
        _offset = _cellSize / 2;
        _gridOriginPosition = new Vector3Int( -_middleCellX  * _cellSize, 0, -_middleCellZ * _cellSize); // this is the world pos of the grids lower left corner so that 0,0 in world space ends up in the middle 
        _walkablbes = new List<Vector3Int>();
        _floorTiles = new Grid<DungeonFloorTile>( _width, _height, _cellSize, _gridOriginPosition, () => new DungeonFloorTile(_floorTiles, _width, _height), false);
        _thisDungeonInstance = SceneManager.GetActiveScene().name;
        _dungeonSeed = (int)OverworldMapData.GetType().GetField(_thisDungeonInstance + "_seed").GetValue(OverworldMapData); // reflection
    }
    /// <summary>
    /// Select a tile from a list of tilepositions
    /// </summary>
    /// <param name="walkables">The list to be parsed</param>
    /// <param name="index">The position in the noise function</param>
    /// <param name="seed">The seed of the noise function</param>
    /// <returns></returns>
    Vector3Int SelectRandomFromWalkableList(List<Vector3Int> walkables, int index, int seed){
        int length = walkables.Count;
        int tileIndex = _noise.IntNoiseInRange(0, length, index, seed);
        Vector3Int tile = walkables[tileIndex];
        return tile;
    }
    /// <summary>
    /// Gets the x (_x) and z (_z) coordinates of the tile we hover over
    /// </summary>
    void GetGridCoords(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPos;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _floorMeshBitMask)){
            worldPos = raycastHit.point;
            _floorTiles.GetXY(worldPos, out _x, out _z);
        }   
    }
    /// <summary>
    /// Updates the player state
    /// </summary>
    /// <param name="state">The state to which we will upgrade the player state</param>
    public void UpdateCurrentPlayerState(PlayerBaseState state){
        _playerState = state;
    }

    void RecordTile(InputAction.CallbackContext context){
        switch (_playerState)
        {
            case PlayerIdleState:
                break;
            case PlayerPlanMoveState:
                Debug.Log(OnMovementTileSelected());
                break;
            default:
                Debug.Log("default");
                break;
        }
    }
    
    Vector3 OnMovementTileSelected(){
        return _floorTiles.GetWorldPosition(_x, _z);
    }



#region SMOOTHING
    void CountSurroundingWalkableTiles(int gridX, int gridZ){
        int walkableCount = 0;

        for (int x = gridX - 1; x < gridX + 1; x++){
            for (int z = gridZ - 1; z < gridZ + 1; z++){
                if (x >= 0 && x < _floorTiles.GetWitdth() && z >= 0 && z < _floorTiles.GetHeight()){
                    if (x != gridX || z != gridZ){
                        if (_floorTiles.GetGridObject(x, z).IsWalkable()){
                            walkableCount++;
                        }
                    }
                }
            }
        }

        _floorTiles.GetGridObject(gridX, gridZ).SetWalkableNeighbourCount(walkableCount);
    }

    void UpdateSurroundingWalkables(){
        for (int x = 0; x < _floorTiles.GetWitdth(); x++){
            for (int z = 0; z < _floorTiles.GetHeight(); z++){
                //Debug.Log($"currently on: {x}, {z} and we are walkable {_floorTiles.GetGridObject(x,z).IsWalkable()}");
                CountSurroundingWalkableTiles(x, z);
             
            }
        }
    }

    void SmoothTheMap(){
        for (int x = 0; x < _floorTiles.GetWitdth(); x++){
            for (int z = 0; z < _floorTiles.GetHeight(); z++){
                if(_floorTiles.GetGridObject(x, z).GetWalkableNeighbourCount() > 2){
                    _floorTiles.GetGridObject(x, z).SetTileWalkable();
                } 
                else if (_floorTiles.GetGridObject(x, z).GetWalkableNeighbourCount() < 2){
                    _floorTiles.GetGridObject(x, z).SetTileUnWalkable();
                }
                //Debug.Log($"currently on: {x}, {z} and we are walkable {_floorTiles.GetGridObject(x, z).GetWalkableNeighbourCount()}");
                //Debug.Log($"currently on: {x}, {z} and we are walkable {_floorTiles.GetGridObject(x,z).IsWalkable()}");
            }
        }
    }

    #endregion

        

}
