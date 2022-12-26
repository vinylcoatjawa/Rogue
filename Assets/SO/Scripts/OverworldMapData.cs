using System.Xml.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "OverworldMapData", menuName = "Overworld/Map/OverworldMapData")]
/// <summary>
/// SO to hold data about the overworld map and the dungeons found on it
/// </summary>
public class OverworldMapData : ScriptableObject
{
    #region General Overworld Map Data
    public int Seed;
    public string mapName;
    #endregion

    #region Dungeon 0
    public string Dungeon_0_name;
    public int Dungeon_0_seed;
    public string Dungeon_0_icon_type; 

    #endregion
    
    #region Dungeon 1
    public string Dungeon_1_name;
    public int Dungeon_1_seed;
    public string Dungeon_1_icon_type;

    #endregion

    #region Dungeon 2
    public string Dungeon_2_name;
    public int Dungeon_2_seed;
    public string Dungeon_2_icon_type;

    #endregion

    #region Dungeon 3
    public string Dungeon_3_name;
    public int Dungeon_3_seed;
    public string Dungeon_3_icon_type;

    #endregion

    #region Dungeon 4
    public string Dungeon_4_name;
    public int Dungeon_4_seed;
    public string Dungeon_4_icon_type;

    #endregion
}