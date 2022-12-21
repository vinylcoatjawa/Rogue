using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   

    public void StartGame()
    {
        Debug.Log("Start was clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
