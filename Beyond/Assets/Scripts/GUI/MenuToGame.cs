using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By Ryan Grayson
 */
public class MenuToGame : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
