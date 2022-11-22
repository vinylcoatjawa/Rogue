using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MapTextureGenerator
{
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D _texture = new Texture2D(width, height);
        _texture.SetPixels(colourMap);
        _texture.Apply();
        return _texture;
    } 

    public static Texture2D TextureFromHeightMap(Grid<float> heigthMap)
    {
        int _width = heigthMap.GetWitdth();
        int _height = heigthMap.GetHeight();

        Color[] colourMap = new Color[_width * _height];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                colourMap[y * _width + x] = Color.Lerp(Color.black, Color.white, heigthMap.GetGridObject(x, y));
            }
        }
        return TextureFromColourMap(colourMap, _width, _height);
    }
}
