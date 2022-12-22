using System.Xml.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "OverworldMapData", menuName = "OverworldMapData")]
public class OverworldMapData : ScriptableObject
{
    string _d1InternalName = "Dungeon_1";
    string _d2InternalName = "Dungeon_2";

    #region General Overworld Map Data
    public int Seed;
    public string mapName;
    #endregion

    #region Dungeon - 1
    public string D1InternalName { get => _d1InternalName; }
    #endregion

    #region Dungeon - 2

    public string D1Interna2Name { get => _d2InternalName; }
    #endregion
}
