using System.Xml.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "OverworldMapData", menuName = "OverworldMapData")]
public class OverworldMapData : ScriptableObject
{
    string _d1InternalName = "Dungeon_1";

    #region General Overworld Map Data
    public int Seed;
    public string mapName;
    #endregion

    #region Dungeon - 1
    public string D1InternalName { get => _d1InternalName; }
    public int D1Seed { get => Seed % 10; } 
    #endregion

    #region Dungeon - 2

    public int D2Seed { get => Seed % 100; }
    #endregion
}
