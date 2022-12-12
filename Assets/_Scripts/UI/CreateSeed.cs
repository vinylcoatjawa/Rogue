using UnityEngine;

public class CreateSeed : MonoBehaviour
{

    public OverworldData overworldData;

    int hash;

    public void ReadStringInput(string seedWord)
    {
        overworldData.Seed = seedWord.GetHashCode();
        overworldData.mapName = seedWord;
    }

}
