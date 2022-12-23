using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Collections;
using System.Collections.Generic;

public class WorldMap : MonoBehaviour
{
    public TerrainType[] Regions;
    public OverworldMapData OverworldMapData;
    public GameObject Dung;

    Renderer _textureRenderer;
    Grid<float> _noiseMap;
    Grid<bool> _structures;
    Grid<OverworldStructureCustomTile> _overworldMapStructureGrid;
    List<Vector2> _possibleStructurePositions;
    Vector2 _structurePosition;


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
        //_structurePosition = FindStructurePosition(CheckForSuitableDungeonTile());
        DrawTexture();
        SpawnDungeonIcon();
        CheckForSuitableDungeonTile();
        Display();
     
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
                //float waterp, grassp, mountainp;
                
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
                        else/* if (_noiseMap.GetGridObject(i, j) < 0.8 && _noiseMap.GetGridObject(i, j) > 0.4)*/
                        {
                            grass++;
                        }
                       
                        
                    }
                }
                /*waterp = water / total;
                mountainp = mountain / total;
                grassp = grass / total;*/
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
    private Vector2 FindStructurePosition(List<Vector2> inputList)
    {
        return inputList[Random.Range(0, inputList.Count)];
    }

    private void SpawnDungeonIcon()
    {
        Instantiate(Dung, new Vector3(_structurePosition.x * 60 + 25 , 1, _structurePosition.y * 60 + 25), Quaternion.Euler(0, 180, 0));
    }




    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }

    
    public void Display()
    {
        
        
        for (int x = 0; x < _overworldMapStructureGrid.GetWitdth(); x++)
        {
            for (int z = 0; z < _overworldMapStructureGrid.GetHeight(); z++)
            {
                Debug.Log(_overworldMapStructureGrid.GetGridObject(x, z).GetTotal());
                Debug.Log("At (" + x + ", " + z + ") the water is" + _overworldMapStructureGrid.GetGridObject(x, z).GetWaterPorportion() +
                    " the grass is " + _overworldMapStructureGrid.GetGridObject(x, z).GetGrassPorportion() + " and mountain is " +
                    _overworldMapStructureGrid.GetGridObject(x, z).GetMountainPorportion());
            }
        }
        
    }


}
