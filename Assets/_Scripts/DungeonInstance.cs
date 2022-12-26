using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Script on the spawned scene
/// </summary>
public class DungeonInstance : MonoBehaviour
{
    public OverworldMapData OverworldMapData;
    string _currentDungeon;
    private void Awake()
    {
        _currentDungeon = SceneManager.GetActiveScene().name;
        Debug.Log(_currentDungeon);
    }
}
