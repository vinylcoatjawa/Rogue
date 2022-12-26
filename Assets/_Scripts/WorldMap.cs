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

    Renderer _textureRenderer;
    Grid<float> _noiseMap;
    Grid<OverworldStructureCustomTile> _overworldMapStructureGrid;
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
        _overworldMapStructureGrid = new Grid<OverworldStructureCustomTile>(10, 10, 60, Vector3.zero, () => default, false);
        DrawTexture(); // Draws the map       
        PopulateStrucureGrid();
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

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = _noiseMap.GetGridObject(x, z);
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].height)
                    {
                        _colourMap[z * width + x] = Regions[i].colour;
                        break;
                    }
                }
            }
        }
        _texture = MapTextureGenerator.TextureFromColourMap(_colourMap, width, height);
        _textureRenderer.sharedMaterial.mainTexture = _texture;
    }
    /// <summary>
    /// Populate MapStructureGrid with custom tiles containing informationabout the terrain in the tile
    /// </summary>
    public void PopulateStrucureGrid()
    {
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
    /// Removes water tiles, where over 30% is water, here we do not wish to spawn dungeons
    /// </summary>
    /// <returns>Dictionary of suitable tiles with keys being the coordinate of the tile and the value being the custom tile</returns>
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
    /// <summary>
    /// Chooses a set amount of location-dungeon key value pairs
    /// </summary>
    /// <param name="inputDict">Filtered dungeon dictionary</param>
    /// <param name="numberOfDungeons">Number of dungeons to generate</param>
    /// <returns>The randomly picked location-dungeon pairs</returns>
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
    /// <summary>
    /// Instantiates the clickable dungeon icons on the map, writes data onto SO about the dungeons
    /// </summary>
    private void SpawnDungeonIcons()
    {
        int index = 0;
        foreach (var dungeon in _chosenStructuresDict)
        {   
            string gameObjectName = "Dungeon " + index;
            string sceneName = "Dungeon_" + index;
            
            string SOSeed = sceneName + "_seed";
            string SOName = sceneName + "_name";
            string SOType = sceneName + "_icon_type";

            GameObject Dungeon_1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Dungeon_1.name =  gameObjectName;
            Dungeon_1.transform.position = new Vector3(dungeon.Key.x * 60 + 30, 1, dungeon.Key.y * 60 + 30);
            Dungeon_1.transform.localScale = new Vector3(6, 1, 6);
            Dungeon_1.transform.rotation = Quaternion.Euler(0, 180, 0);
            
            if (dungeon.Value.GetWaterProportion() > 0) {
                Dungeon_1.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/CoastalDungeonIcon");
                OverworldMapData.GetType().GetField(SOType).SetValue(OverworldMapData, "Coastal");
            } else if (dungeon.Value.GetMountainProportion() > 0){
                Dungeon_1.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/MountainDungeonIcon");
                OverworldMapData.GetType().GetField(SOType).SetValue(OverworldMapData, "Mountain");
            } else {
                Dungeon_1.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/StandardDungeonIcon");
                OverworldMapData.GetType().GetField(SOType).SetValue(OverworldMapData, "Standard");
            }
            Dungeon_1.AddComponent<DungeonIcon>();
            var Icon = Dungeon_1.GetComponent<DungeonIcon>();
            Icon.SceneName = sceneName;
            OverworldMapData.GetType().GetField(SOSeed).SetValue(OverworldMapData, OverworldMapData.Seed * (index + 2));
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



    

}
