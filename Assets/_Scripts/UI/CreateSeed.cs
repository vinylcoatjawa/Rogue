using UnityEngine;
public class CreateSeed : MonoBehaviour
{
    public OverworldMapData overworldMapData;
    /// <summary>
    /// Reads input word from which it creates a seed and writes it on SO 
    /// </summary>
    /// <param name="seedWord">The user input read from the Main menu screen</param>
    public void ReadStringInput(string seedWord)
    {
        overworldMapData.Seed = seedWord.GetHashCode();
        overworldMapData.mapName = seedWord;

    }

}
