using UnityEngine;
using NoiseUtils;

public class PerlinMap : MonoBehaviour
{
    /// <summary>
    /// Generates a Perlin noise based map on a 2D grid with float values from 0 to 1
    /// </summary>
    /// <param name="mapWidth">The Width of the map</param>
    /// <param name="mapHeight">The Height of the map</param>
    /// <param name="scale">The Scale/zoom</param>
    /// <param name="octaves">The number of layers of noise to be added together</param>
    /// <param name="persistance">Controls the amplitude of each octave</param>
    /// <param name="lacunarity">Controls the frequency of each octave should be greater than 1</param>
    /// <param name="seed"></param>
    /// <param name="offset"></param>
    /// <returns>A grid of floats based on Perlin noise</returns>
    public static Grid<float> GenerateNoiseMap(int mapWidth, int mapHeight, float cellSize, float scale, int octaves, float persistance, float lacunarity, uint seed, Vector2 offset, bool allowDebug = false)
    {
        Grid<float> noiseMap = new Grid<float>(mapWidth, mapHeight, cellSize, Vector3.zero, () => 0f, allowDebug);
        Noise noise = new Noise();

        Vector2[] octaveOffsets = new Vector2[octaves];
        uint noiseDisturbance = 123;

        for (int octave = 0; octave < octaves; octave++)
        {
            int offsetX = (int)noise.NoiseInRange(0, 200000, (uint)octave * noiseDisturbance + noiseDisturbance, seed) - 100000 + (int)offset.x;
            int offsetY = (int)noise.NoiseInRange(0, 200000, (uint)octave * noiseDisturbance + 2 * noiseDisturbance, seed) - 100000 + (int)offset.y;
            octaveOffsets[octave] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) { scale = 0.00001f; };

        // need to keep track of these so we can rescale our noise map to be between 0 and 1
        float maxNoiseHeight = float.MinValue; 
        float minNoiseHeight = float.MaxValue;

        // these are used to make the Scale change zoom in on the middle of the map
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int octave = 0; octave < octaves; octave++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[octave].x;
                    float sampleY = (y - halfWidth) / scale * frequency + octaveOffsets[octave].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // making perlinValue to go from -1 to 1
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) { maxNoiseHeight = noiseHeight; }
                else if (noiseHeight < minNoiseHeight) { minNoiseHeight = noiseHeight; }

                noiseMap.SetGridObject(x, y, noiseHeight);



                

            }
        }
        // we Scale back the noise map to have values only between 0 and 1
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                noiseMap.SetGridObject(x, y, Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap.GetGridObject(x, y)));
            }
        }
        return noiseMap;
    }


}
