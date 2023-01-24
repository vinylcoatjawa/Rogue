using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An indicator quad highlighting the tile we currently hovering over
/// </summary>
public class MovementIndicator : MonoBehaviour
{
    MeshFilter _meshFilter;
    MeshRenderer _meshRenderer;
    Vector3 _lowerLeftVertice;
    Mesh _movementIndicatorMesh;
    Vector3[] _vertices; 
    Vector2[] _uvs; 
    int[] _triangles;

    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _movementIndicatorMesh = new Mesh();

    }
    /// <summary>
    /// Creates a quad over a dungeon floor tile to indicate the target tile of the move
    /// </summary>
    /// <param name="input">A Vector3 which is the world space coordinates of the lower left corner of the dungeon floor grid tile</param>
    public void SpawnMovementIndicator(Vector3 input){
        _lowerLeftVertice = input;
        GetComponent<MeshFilter>().mesh = _movementIndicatorMesh; 

        _vertices = new Vector3[4];
        _uvs = new Vector2[4];
        _triangles = new int[6];
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;

        _vertices[0] = _lowerLeftVertice + new Vector3(0, 0.1f, 0);
        _vertices[1] = _lowerLeftVertice + new Vector3(0, 0.1f, 10);
        _vertices[2] = _lowerLeftVertice + new Vector3(10, 0.1f, 10);
        _vertices[3] = _lowerLeftVertice + new Vector3(10, 0.1f, 0);

        _uvs[0] = new Vector2(uv00.x, uv00.y) ;
        _uvs[1] = new Vector2(uv00.x, uv11.y) ;
        _uvs[2] = new Vector2(uv11.x, uv11.y) ;
        _uvs[3] = new Vector2(uv11.x, uv00.y) ;

        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = 2;
        
        _triangles[3] = 0;
        _triangles[4] = 2;
        _triangles[5] = 3;

        _movementIndicatorMesh.vertices = _vertices;
        _movementIndicatorMesh.uv = _uvs;
        _movementIndicatorMesh.triangles = _triangles;
        _movementIndicatorMesh.RecalculateNormals();
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks 1");
        _meshRenderer.material = mat;
    }
    /// <summary>
    /// Removes the spawned quad by clearing the mesh
    /// </summary>
    public void Destroy(){
        GetComponent<MeshFilter>().mesh.Clear();
    }

    

}
