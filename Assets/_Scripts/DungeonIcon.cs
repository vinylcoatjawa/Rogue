using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonIcon : MonoBehaviour
{
    Renderer _meshRenderer;
    Color _originalColor;

    public string SceneName;
    
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
        SceneManager.LoadScene(SceneName);
    }
}
