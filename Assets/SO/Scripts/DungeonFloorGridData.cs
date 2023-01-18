using UnityEngine;

[CreateAssetMenu(fileName = "DungeonFloorGridData", menuName = "SO/Dungeon Data/Dungeon Floor Grid")]
public class DungeonFloorGridData : ScriptableObject
{
    public int GridWidth;
    public int GridHeight;
    public int CellSize;
    public int DungeonFloorCount;
    public Vector3Int GridOriginPosition;

}
