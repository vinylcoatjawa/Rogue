using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonInstance : MonoBehaviour
{
    public OverworldMapData OverworldMapData;

    string currentDungeon;

    private void Awake()
    {
        currentDungeon = SceneManager.GetActiveScene().name;
        Debug.Log(currentDungeon);
    }


}
