using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class DungeonIcon : MonoBehaviour
{
    public ScriptableObject DungeonData;
    
    Renderer _meshRenderer;
    Color _originalColor;
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
    }


    private void OnMouseEnter()
    {
        _meshRenderer.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        _meshRenderer.material.color = _originalColor;
    }
    private void OnMouseUp()
    {

        //SceneManager.LoadScene(DungeonData.dungeonName);
        SceneManager.LoadScene("Dungeon_1");
    }
}
