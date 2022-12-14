using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int randint;
    OverWorldMapDisplay overworldMapDisplay;

    private void Awake()
    {
        overworldMapDisplay = GetComponent<OverWorldMapDisplay>();
    }

    public void StartGame()
    {
        Debug.Log("Start was clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        overworldMapDisplay.Draw();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
