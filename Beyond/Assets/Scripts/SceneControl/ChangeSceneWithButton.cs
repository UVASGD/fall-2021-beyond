using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * By Ryan Grayson
 */
public class ChangeSceneWithButton : MonoBehaviour
{
    public Animator transition;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(wait(sceneName));
    }

    IEnumerator wait(string sceneName){
        transition.SetTrigger("GameOver");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
    }
}
