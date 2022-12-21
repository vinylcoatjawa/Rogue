using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInstance : MonoBehaviour
{
    public OverworldMapData OverworldMapData;
    private void Awake()
    {
        Debug.Log("Dungeon 1 is loaded");
        Debug.Log(OverworldMapData.D1Seed + "  " + OverworldMapData.D2Seed);
    }
}
