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
                overworldMapDisplay.DrawNoiseMap(overworldMapDisplay.GenerateNoiseMap());
            }
        }

            if (GUILayout.Button("Start"))
        {
            overworldMapDisplay.DrawNoiseMap(overworldMapDisplay.GenerateNoiseMap());

        }

    }
}
