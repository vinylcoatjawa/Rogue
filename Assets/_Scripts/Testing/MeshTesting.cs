using UnityEngine.InputSystem;
using UnityEngine;

/*          MAY BE OBSOLETE         */

public class MeshTesting : MonoBehaviour
{
    Mesh _mesh;
    MeshFilter meshFilter;

    int gridHeight = 5, gridWidth = 5, cellSize = 10;

    Grid<bool> testGrid;
    Grid<DungeonFloorTile> dungTiles;

    private void Start() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        testGrid = new Grid<bool>(gridWidth, gridHeight, cellSize, Vector3.zero, () => false, true);       
        MeshUtils.CreateEmptyMeshArrays(25, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);
        Vector3 quadSize = new Vector3(1,1) * cellSize;
        
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                int index = x * gridHeight + z;
                
                Vector3 pos = testGrid.GetWorldPosition(x, z);
                          
                int vIndex = index*4;
                int vIndex0 = vIndex;
                int vIndex1 = vIndex+1;
                int vIndex2 = vIndex+2;
                int vIndex3 = vIndex+3;


                vertices[vIndex0] = pos + Vector3.zero;
                vertices[vIndex1] = pos + new Vector3(0, 0, cellSize);
                vertices[vIndex2] = pos + new Vector3(cellSize, 0, cellSize);
                vertices[vIndex3] = pos + new Vector3(cellSize, 0, 0);
                

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

            }
        }

        
        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
        Renderer rend = GetComponent<MeshRenderer>();
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
        rend.material = mat;
        
        
    }


    public void DrawDungeonFloor(){
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        testGrid = new Grid<bool>(gridWidth, gridHeight, cellSize, Vector3.zero, () => true, true);       
        MeshUtils.CreateEmptyMeshArrays(25, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

        Vector3 quadSize = new Vector3(1,1) * cellSize;
        
        Vector2 uv00 = Vector2.zero;
        Vector2 uv11 = Vector2.one;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                int index = x * gridHeight + z;
                
                Vector3 pos = testGrid.GetWorldPosition(x, z);
                          
                int vIndex = index*4;
                int vIndex0 = vIndex;
                int vIndex1 = vIndex+1;
                int vIndex2 = vIndex+2;
                int vIndex3 = vIndex+3;


                vertices[vIndex0] = pos + Vector3.zero;
                vertices[vIndex1] = pos + new Vector3(0, 0, cellSize);
                vertices[vIndex2] = pos + new Vector3(cellSize, 0, cellSize);
                vertices[vIndex3] = pos + new Vector3(cellSize, 0, 0);
                

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

            }
        }

        
        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
        Renderer rend = GetComponent<MeshRenderer>();
        Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
        rend.sharedMaterial = mat;
    }
    
    public void TestingMouseInput(InputAction.CallbackContext context){
        if(context.performed){
            Debug.Log("clicked");
            GetComponent<MeshFilter>().mesh = _mesh;
            //GetComponent<MeshFilter>().mesh.uv = _uv;
            Renderer rend = GetComponent<MeshRenderer>();
            Material mat = Resources.Load<Material>("Materials/lowP/Vol_23_1_Rocks");
            rend.material = mat;
            
        }



        
    }



}
