using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonIcon : MonoBehaviour
{
    public OverworldMapData OverworldMapData;
    public DungeonInstanceData DungeonInstanceData;
    
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
        SceneManager.LoadScene(OverworldMapData.D1InternalName);
    }
}
