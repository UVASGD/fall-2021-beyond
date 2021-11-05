using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * By Eric Weng
 * !Deprecated!
 */
public class OverlayController : MonoBehaviour
{
    private static OverlayController INSTANCE = null;

    // should be singleton/static?
    // TODO maybe fade in/out once entering/exiting
    [SerializeField] private List<GameObject> triggers;
    [SerializeField] private List<TextMeshProUGUI> overlays;

    void Start()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        INSTANCE.overlays.ForEach(delegate(TextMeshProUGUI text)
        {
            text.gameObject.SetActive(false);
        });
    }

    public static void SetTextBoxActive(string triggerName, bool active)
    {
        GameObject triggerSrc = GameObject.Find(triggerName);
        int index = INSTANCE.triggers.IndexOf(triggerSrc);
        TextMeshProUGUI textBox = INSTANCE.overlays[index];
        if (textBox != null)
        {
            textBox.gameObject.SetActive(active);
        }
    }

}
