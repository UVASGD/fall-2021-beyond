using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By Ryan Grayson and Eric Weng
 */
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Animator optionalTransition = null;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        if (optionalTransition != null)
        {
            optionalTransition.SetTrigger("GameOver");
            yield return new WaitForSeconds(2);
        }
        SceneManager.LoadScene(sceneName);
    }
}
