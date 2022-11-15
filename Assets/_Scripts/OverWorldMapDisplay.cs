using UnityEngine;


public class OverWorldMapDisplay : MonoBehaviour
{
    Renderer textureRenderer;

    public bool AutoUpdate;

    public int height;
    public int width;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public uint seed;
    public int offsetX;
    public int offsetY;

    Grid<float> _noiseMap;


    public Grid<float> GenerateNoiseMap()
    {
        Vector2 offset = new Vector2(offsetX, offsetY);
        _noiseMap = PerlinMap.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, seed, offset);
        return _noiseMap;
    }

    public void DrawNoiseMap(Grid<float> noiseMap)
    {
        
        textureRenderer = GetComponent<Renderer>();

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap.GetGridObject(x, y));
            }
        }
        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
