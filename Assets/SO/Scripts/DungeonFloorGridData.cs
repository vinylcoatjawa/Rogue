using UnityEngine;

[CreateAssetMenu(fileName = "DungeonFloorGridData", menuName = "SO/Dungeon Data/Dungeon Floor Grid")]
public class DungeonFloorGridData : ScriptableObject
{
    public int GridWidth;
    public int GridHeight;
    public int CellSize;
}
