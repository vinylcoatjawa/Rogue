using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OverWorldMapDisplay))]
public class OverwoldMapEditor : Editor
{


    
    public override void OnInspectorGUI()
    {

        OverWorldMapDisplay overworldMapDisplay = (OverWorldMapDisplay)target;

        //base.OnInspectorGUI();

        if (DrawDefaultInspector())
        {
            if (overworldMapDisplay.AutoUpdate)
            {
                overworldMapDisplay.DrawTexture();
            }
        }


        if (GUILayout.Button("Start_1"))
        {
            overworldMapDisplay.DrawTexture();

        }
        if (GUILayout.Button("Structures"))
        {
            overworldMapDisplay.GenerateStructures();

        }
    }
}
