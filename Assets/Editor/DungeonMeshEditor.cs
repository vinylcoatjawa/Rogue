using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonMeshTest))]
public class DungeonMeshEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {

        DungeonMeshTest dungeonMeshTest = (DungeonMeshTest)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Data Init"))
        {
            dungeonMeshTest.DataInit();

        }
        
        if (GUILayout.Button("Genrate"))
        {
            dungeonMeshTest.GenerateMesh();

        }
    }
    */
    void OnSceneGUI()
    {

        
        DungeonMeshTest dungeonMeshTest = (DungeonMeshTest)target;

        if (dungeonMeshTest._mesh == null){
            Debug.Log("in");
        dungeonMeshTest.DataInit();
        dungeonMeshTest.GenerateMesh();
        }
        else return;
        
        /*
        base.OnInspectorGUI();

        if (GUILayout.Button("Data Init"))
        {
            dungeonMeshTest.DataInit();

        }
        
        if (GUILayout.Button("Genrate"))
        {
            dungeonMeshTest.GenerateMesh();

        }
        */
    }
}
