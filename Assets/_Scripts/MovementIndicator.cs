using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    MeshFilter _meshFilter;
    MeshRenderer _meshRenderer;
    Vector3 _lowerLeftVertice;
    Mesh _movementIndicatorMesh;
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _movementIndicatorMesh = new Mesh();

    }

    public void FindLowerLeftVertice(Vector3 input){
        _lowerLeftVertice = input;
        GetComponent<MeshFilter>().mesh = _movementIndicatorMesh; 
        //Debug.Log($"from ind {_lowerLeftVertice}");

        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;

        vertices[0] = _lowerLeftVertice + new Vector3(0, 0.1f, 0);
        vertices[1] = _lowerLeftVertice + new Vector3(0, 0.1f, 10);
        vertices[2] = _lowerLeftVertice + new Vector3(10, 0.1f, 10);
        vertices[3] = _lowerLeftVertice + new Vector3(10, 0.1f, 0);

        uvs[0] = new Vector2(uv00.x, uv00.y) ;
        uvs[1] = new Vector2(uv00.x, uv11.y) ;
        uvs[2] = new Vector2(uv11.x, uv11.y) ;
        uvs[3] = new Vector2(uv11.x, uv00.y) ;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        _movementIndicatorMesh.vertices = vertices;
        _movementIndicatorMesh.uv = uvs;
        _movementIndicatorMesh.triangles = triangles;
        _movementIndicatorMesh.RecalculateNormals();
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks 1");
        _meshRenderer.material = mat;
    }

    

}
