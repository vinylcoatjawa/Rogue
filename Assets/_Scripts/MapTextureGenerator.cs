using UnityEngine;
/// <summary>
/// Texture generation for both height map and colour map
/// </summary>
public static class MapTextureGenerator
{
    /// <summary>
    /// Texture generator from a given colour map
    /// </summary>
    /// <param name="colourMap"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns>Texture</returns>
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    } 
    /// <summary>
    /// Texture generation from a given 2D grid of height values 
    /// </summary>
    /// <param name="heightMap"></param>
    /// <returns></returns>
    public static Texture2D TextureFromHeightMap(Grid<float> heightMap)
    {
        int width = heightMap.GetWitdth();
        int height = heightMap.GetHeight();
        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap.GetGridObject(x, y));
            }
        }
        return TextureFromColourMap(colourMap, width, height);
    }
}
