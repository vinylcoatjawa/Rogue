using UnityEngine;

public class CreateSeed : MonoBehaviour
{

    public OverworldMapData overworldMapData;

    
    

    public void ReadStringInput(string seedWord)
    {
        overworldMapData.Seed = seedWord.GetHashCode();
        overworldMapData.mapName = seedWord;

    }

}
