using System;
using UnityEngine;


public class OverWorldMapDisplay : MonoBehaviour
{
    Renderer textureRenderer;

    public bool AutoUpdate;

    public int Height;
    public int Width;
    public float Scale;
    public int Octaves;
    [Range(0,1)]
    public float Persistance;
    public float Lacunarity;
    public uint Seed;
    public int OffsetX;
    public int OffsetY;

    Grid<float> _noiseMap;


    public Grid<float> GenerateNoiseMap()
    {
        Vector2 offset = new Vector2(OffsetX, OffsetY);
        _noiseMap = PerlinMap.GenerateNoiseMap(Width, Height, Scale, Octaves, Persistance, Lacunarity, Seed, offset, false);
        return _noiseMap;
    }

    public void DrawNoiseMap(Grid<float> noiseMap)
    {
        
        textureRenderer = GetComponent<Renderer>();

        Texture2D texture = new Texture2D(Width, Height);

        Color[] colourMap = new Color[Width * Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                colourMap[y * Width + x] = Color.Lerp(Color.black, Color.white, noiseMap.GetGridObject(x, y));
            }
        }
        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(Width, 1, Height);
    }

    private void OnValidate()
    {
        if (Width < 1) { Width = 1; }
        if (Height < 1) { Height = 1; }
        if (Lacunarity < 1) { Lacunarity = 1; }
        if (Octaves < 0) { Octaves = 0; }
    }
}