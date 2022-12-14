using UnityEngine;

public class CreateSeed : MonoBehaviour
{

    public OverworldData overworldData;

    
    

    public void ReadStringInput(string seedWord)
    {
        overworldData.Seed = seedWord.GetHashCode();
        overworldData.mapName = seedWord;

    }

}
