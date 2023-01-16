using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMeshTest : MonoBehaviour
{
    public DungeonFloorGridData DungeonFloorGridData;
    public Mesh _mesh;
    MeshFilter _meshFilter;
    Grid<DungeonFloorTile> _dungTiles;
    int _width, _height, _cellSize;
    Renderer _rend;
    Material _mat;

    public void DataInit(){
        _width = DungeonFloorGridData.GridWidth;
        _height = DungeonFloorGridData.GridHeight;
        _cellSize = DungeonFloorGridData.CellSize;
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _rend = GetComponent<MeshRenderer>();
        _dungTiles = new Grid<DungeonFloorTile>(_width, _height, _cellSize, Vector3.zero, () => new DungeonFloorTile(_dungTiles, _width, _height), true);
    }

    public void GenerateMesh(){

        MeshUtils.CreateEmptyMeshArrays(_width * _height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        Vector3 quadSize = new Vector3(1,1) * _cellSize;
        
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;

        

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                int index = x * _height + z;
                
                Vector3 pos = _dungTiles.GetWorldPosition(x, z);

                Debug.Log($"world pos: {pos} for ({x}, {z})");

                int vIndex = index*4;
                int vIndex0 = vIndex;
                int vIndex1 = vIndex+1;
                int vIndex2 = vIndex+2;
                int vIndex3 = vIndex+3;


                vertices[vIndex0] = pos + Vector3.zero;
                vertices[vIndex1] = pos + new Vector3(0, 0, _cellSize);
                vertices[vIndex2] = pos + new Vector3(_cellSize, 0, _cellSize);
                vertices[vIndex3] = pos + new Vector3(_cellSize, 0, 0);
                

                uvs[vIndex0] = new Vector2(uv00.x, uv00.y) ;
                uvs[vIndex1] = new Vector2(uv00.x, uv11.y) ;
                uvs[vIndex2] = new Vector2(uv11.x, uv11.y) ;
                uvs[vIndex3] = new Vector2(uv11.x, uv00.y) ;


                int tIndex = index*6;

                triangles[tIndex+0] = vIndex0;
                triangles[tIndex+1] = vIndex1;
                triangles[tIndex+2] = vIndex2;
                
                triangles[tIndex+3] = vIndex0;
                triangles[tIndex+4] = vIndex2;
                triangles[tIndex+5] = vIndex3;

                Debug.Log($"index is: {index} and  v1 is: {vertices[vIndex0]}, v2 is: {vertices[vIndex1]}");

            }
        }

        Debug.Log($"{_mesh} and {vertices.Length}");
        
        _mesh.vertices = vertices;
        Debug.Log("hej");
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
        _mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
        _rend.sharedMaterial = _mat;




    }



}
