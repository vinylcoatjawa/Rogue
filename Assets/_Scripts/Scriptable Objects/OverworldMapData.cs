using System.Xml.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "OverworldMapData", menuName = "OverworldMapData")]
public class OverworldMapData : ScriptableObject
{
    public string _d1InternalName = "Dungeon_1";

    #region General Overworld Map Data
    public int Seed;
    public string mapName;
    #endregion

    #region Dungeon - 1
    public int D1Seed { get { return Seed % 10; } }
    public string D1InternalName { get { return _d1InternalName; } }
    #endregion

    #region Dungeon - 2

    public int D2Seed { get { return Seed % 100; } }
    #endregion
}
