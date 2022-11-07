using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridTester))]
public class GridTestEditor : Editor
{
    public override void OnInspectorGUI()
    {

        GridTester gridTester = (GridTester)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Start"))
        {
            //Debug.Log("hej");
            gridTester.printCoords();
        }
    }
}
