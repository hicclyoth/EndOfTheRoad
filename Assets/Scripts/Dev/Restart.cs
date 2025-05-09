using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene
    }
}
