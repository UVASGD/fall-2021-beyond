using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By Ryan Grayson and Eric Weng
 */
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Animator transition = null; // optional

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        if (transition != null)
        {
            Debug.Log("Transitioning");
            transition.SetTrigger("GameOver");
            yield return new WaitForSeconds(2);
        }
        SceneManager.LoadScene(sceneName);
    }
}
