using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFindTester))]
public class PathFindingEditor : Editor
{


    
    public override void OnInspectorGUI()
    {

        PathFindTester _pathFinding = (PathFindTester)target;

        //base.OnInspectorGUI();
        /*
        if (DrawDefaultInspector())
        {
            if (overworldMapDisplay.AutoUpdate)
            {
                overworldMapDisplay.DrawTexture();
            }
        }
        */

        if (GUILayout.Button("Init")){
            _pathFinding.Init();
        }
        if (GUILayout.Button("Run")){
            _pathFinding.RunThrough();
        }
        
        
    }
}
