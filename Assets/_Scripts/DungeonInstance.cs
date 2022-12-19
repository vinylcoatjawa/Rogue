using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInstance : MonoBehaviour
{
    public ScriptableObject DungeonOneData;
    private void Awake()
    {
        Debug.Log("Dungeon 1 is loaded");
    }
}
