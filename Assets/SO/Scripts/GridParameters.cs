using UnityEngine;

[CreateAssetMenu(fileName = "GridParameters", menuName = "SO/General/Grid Parameters")]
public class GridParameters : ScriptableObject
{
    public int GridWidth;
    public int GridHeight;
    public int CellSize;
    public Vector3Int GridOriginPosition;

}
