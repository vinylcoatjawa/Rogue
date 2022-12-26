using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.SceneManagement;

public class WorldMap : MonoBehaviour
{
    public TerrainType[] Regions;
    public OverworldMapData OverworldMapData;
    public GameObject Dung;

    string[] DungeonNameList = { "Dungeon 1", "Dungeon 2", "Dungeon 3", "Dungeon 4", "Dungeon 5" };

    Renderer _textureRenderer;
    Grid<float> _noiseMap;
    Grid<bool> _structures;
    Grid<OverworldStructureCustomTile> _overworldMapStructureGrid;
    List<OverworldStructureCustomTile> _possibleStructurePositions;
    Vector2 _structurePosition;
    Dictionary<Vector2, OverworldStructureCustomTile> _possibleStructureDict = new Dictionary<Vector2, OverworldStructureCustomTile>();
    Dictionary<Vector2, OverworldStructureCustomTile> _chosenStructuresDict = new Dictionary<Vector2, OverworldStructureCustomTile>();

    private void Awake()
    {
        _textureRenderer = GetComponent<Renderer>();
        if (OverworldMapData.Seed == 0) { OverworldMapData.Seed = Random.Range(int.MinValue, int.MaxValue); } // making sure to generate map with random seed even if no input was given       
    }
    private void Start()
    {
        _noiseMap = PerlinMap.GenerateNoiseMap(360, 360, 10 , 165, 4, 0.33f, 2.5f, (uint)OverworldMapData.Seed, Vector2.zero, false);
        _structures = new Grid<bool>(10, 10, 60, Vector3.zero, () => false, false);
        _overworldMapStructureGrid = new Grid<OverworldStructureCustomTile>(10, 10, 60, Vector3.zero, () => default, false);
        DrawTexture();
        
        CheckForSuitableDungeonTile();
        _possibleStructureDict = FilterAwaySeaTiles();
        _chosenStructuresDict = FindStructurePosition(_possibleStructureDict, 5);
        SpawnDungeonIcons();
    }

    /// <summary>
    /// Creates the world map texture and draws it on the gameobject
    /// </summary>
    public void DrawTexture()
    {
        _textureRenderer = GetComponent<Renderer>();

        int width = _noiseMap.GetWitdth();
        int height = _noiseMap.GetHeight();

        Texture2D _texture;

        Color[] _colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = _noiseMap.GetGridObject(x, y);
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].height)
                    {
                        _colourMap[y * width + x] = Regions[i].colour;
                        break;
                    }
                }
            }
        }
        _texture = MapTextureGenerator.TextureFromColourMap(_colourMap, width, height);
        _textureRenderer.sharedMaterial.mainTexture = _texture;
    }
    /// <summary>
    /// Map is divided up to 100 tiles and checked whether we have water in those, if not the coordinates are saved in a list
    /// </summary>
    /// <returns>List of suitable dungeon positions as vector 2s</returns>
    public void CheckForSuitableDungeonTile()
    {
        
        
        List<OverworldStructureCustomTile> possibleStructurePositions = new List<OverworldStructureCustomTile>();

        for (int x = 0; x < _overworldMapStructureGrid.GetWitdth(); x++)
        {
            for (int z = 0; z < _overworldMapStructureGrid.GetHeight(); z++)
            {
                int water = 0, grass = 0, mountain = 0, total = 0;
                
                for (int i = x * 36; i < (x + 1) * 36; i++) // currently 36 is the hard coded size difference between the underlying perlin noise and the structure grid
                {
                    for (int j = z * 36; j < (z + 1) * 36; j++)
                    {
                        total++;
                        if (_noiseMap.GetGridObject(i, j) <= 0.4)
                        {
                            water++;
                        }
                        else if (_noiseMap.GetGridObject(i, j) >= 0.8)
                        {
                            mountain++;
                        }
                        else
                        {
                            grass++;
                        }        
                    }
                }
                OverworldStructureCustomTile current = new OverworldStructureCustomTile(total, grass, mountain, water);
                _overworldMapStructureGrid.SetGridObject(x, z, current);

            }
        }
        
    }
    /// <summary>
    /// Picks a random vector 2 out of a list of vector 2s
    /// </summary>
    /// <param name="inputList">List to pick a random element out of</param>
    /// <returns>The randomly picked vector 2</returns>
    private Dictionary<Vector2, OverworldStructureCustomTile> FindStructurePosition(Dictionary<Vector2, OverworldStructureCustomTile> inputDict, int numberOfDungeons = 1)
    {
        List<Vector2> keyList = new List<Vector2>(inputDict.Keys);
        Dictionary<Vector2, OverworldStructureCustomTile> returnDict = new Dictionary<Vector2, OverworldStructureCustomTile>();

        for (int i = 0; i < numberOfDungeons; i++)
        {

            Vector2 currKey = keyList[Random.Range(0, keyList.Count)];
            returnDict.Add(currKey, inputDict[currKey]);
            keyList.Remove(currKey);
        }
        return returnDict;
    }

    private void SpawnDungeonIcons()
    {
        int index = 0;

        foreach (var dungeon in _chosenStructuresDict)
        {
            

            string gameObjectName = "Dungeon " + index;
            string sceneName = "Dungeon_" + index;

            GameObject Dungeon_1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Dungeon_1.name =  gameObjectName;
            Dungeon_1.transform.position = new Vector3(dungeon.Key.x * 60 + 30, 1, dungeon.Key.y * 60 + 30);
            Dungeon_1.transform.localScale = new Vector3(6, 1, 6);
            Dungeon_1.transform.rotation = Quaternion.Euler(0, 180, 0);
            
            if (dungeon.Value.GetWaterProportion() > 0) {
                Dungeon_1.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/CoastalDungeonIcon");
            } else if (dungeon.Value.GetMountainProportion() > 0){
                Dungeon_1.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/MountainDungeonIcon");
            } else Dungeon_1.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/StandardDungeonIcon");
            
            Dungeon_1.AddComponent<DungeonIcon>();
            var Icon = Dungeon_1.GetComponent<DungeonIcon>();
            Icon.SceneName = sceneName;

            
            
            index++;
        } 
         
    }



    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }



    private Dictionary<Vector2, OverworldStructureCustomTile> FilterAwaySeaTiles()
    {
        Dictionary<Vector2, OverworldStructureCustomTile> filteredDict = new Dictionary<Vector2, OverworldStructureCustomTile>();


        for (int x = 0; x < _overworldMapStructureGrid.GetWitdth(); x++)
        {
            for (int z = 0; z < _overworldMapStructureGrid.GetHeight(); z++)
            {
                if (_overworldMapStructureGrid.GetGridObject(x, z).GetWaterProportion() < 0.3)
                {

                    filteredDict.Add(new Vector2(x, z), _overworldMapStructureGrid.GetGridObject(x, z));
                    

                } 
            }
        }
        return filteredDict;
    }

}
