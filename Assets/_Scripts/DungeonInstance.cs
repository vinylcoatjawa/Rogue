using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonInstance : MonoBehaviour
{
    public OverworldMapData OverworldMapData;
    public DungeonInstanceData DungeonInstanceData;

    string _currentDungeon;
    int _dungeonSeed;

    private void Awake()
    {
        DungeonInstanceData.dungeonSeed = OverworldMapData.Seed % 10;



        _currentDungeon = SceneManager.GetActiveScene().name;
        Debug.Log(_currentDungeon + " with seed " + _dungeonSeed);
    }


}
