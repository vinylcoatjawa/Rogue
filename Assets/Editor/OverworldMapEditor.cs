using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OverWorldMapDisplay))]
public class OverwoldMapEditor : Editor
{

    Grid<float> noiseMap;

    


    
    public override void OnInspectorGUI()
    {

        OverWorldMapDisplay overworldMapDisplay = (OverWorldMapDisplay)target;

        //base.OnInspectorGUI();

        if (DrawDefaultInspector())
        {
            if (overworldMapDisplay.AutoUpdate)
            {
                overworldMapDisplay.DisplayMap();
            }
        }

            if (GUILayout.Button("Start"))
        {
            overworldMapDisplay.DisplayMap();

        }
        if (GUILayout.Button("Start_1"))
        {
            overworldMapDisplay.DrawTexture();

        }
    }
}
