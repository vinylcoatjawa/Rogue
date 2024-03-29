using System;
using UnityEngine;


public class OverWorldMapDisplay : MonoBehaviour
{
    
    public enum DrawMode { NoiseMap, ColourMap };

    public bool AutoUpdate;
    public TerrainType[] Regions;
    public DrawMode CurrentDrawMode;
    public int Height = 60;
    public int Width = 60;
    public int CellSize = 10;
    public float Scale;
    public int Octaves;
    [Range(0,1)]
    public float Persistance;
    public float Lacunarity;
    public uint Seed;
    public int OffsetX;
    public int OffsetY;
    public bool AllowDebug = false;
    public bool AllowStructDebug;

    Renderer _textureRenderer;
    Grid<float> _noiseMap;
    Grid<int> _structures;


    public Grid<float> GenerateNoiseMap()
    {
        Vector2 offset = new Vector2(OffsetX, OffsetY);
        _noiseMap = PerlinMap.GenerateNoiseMap(Width, Height, CellSize, Scale, Octaves, Persistance, Lacunarity, Seed, offset, AllowDebug);
        //_noiseMap = PerlinMap.GenerateNoiseMap(60, 60, 10, 165, 4, 0.33f, 2.5f, 159, new Vector2(898,37), false);
        return _noiseMap;
    }

    public void DrawTexture()
    {
        
        _textureRenderer = GetComponent<Renderer>();

        Grid<float> _noiseMap = GenerateNoiseMap();
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

        
        if (CurrentDrawMode == DrawMode.NoiseMap)
        {
            _texture = MapTextureGenerator.TextureFromHeightMap(_noiseMap);
            _textureRenderer.sharedMaterial.mainTexture = _texture;
        }
        else if (CurrentDrawMode == DrawMode.ColourMap)
        {
            _texture = MapTextureGenerator.TextureFromColourMap(_colourMap, width, height);
            _textureRenderer.sharedMaterial.mainTexture = _texture;
        }
         
        
    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }

    private void OnValidate()
    {
        if (Width < 1) { Width = 1; }
        if (Height < 1) { Height = 1; }
        if (Lacunarity < 1) { Lacunarity = 1; }
        if (Octaves < 0) { Octaves = 0; }
    }

    public void GenerateStructures()
    {
        float width = (float)Width / 10;
        float height = (float)Height / 10;
        //float cellSize = CellSize * Height;
        //_structures = new Grid<int>((int)width, (int)height, CellSize * 6, Vector3.zero, () => 0, AllowStructDebug);
        //_structures = new Grid<int>(10, 10, 60, Vector3.zero, () => 0, AllowStructDebug);
    }
}
