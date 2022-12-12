using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public OverworldData overworldData;

    public void StartGame()
    {
        Debug.Log("Start was clicked");
        overworldData.Seed = 15;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("I quit");
        Application.Quit();
    }
}
