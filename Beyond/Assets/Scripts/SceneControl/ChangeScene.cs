using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By Ryan Grayson and Eric Weng
 */
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Animator optionalTransition = null; // optional

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        if (optionalTransition != null)
        {
            Debug.Log("Transitioning");
            optionalTransition.SetTrigger("GameOver");
            yield return new WaitForSeconds(2);
        }
        SceneManager.LoadScene(sceneName);
    }
}
