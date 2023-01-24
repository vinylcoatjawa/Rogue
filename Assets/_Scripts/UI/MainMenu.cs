using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Main menu handling script
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// The start buttons functionality
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// The quit buttons functionality
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
