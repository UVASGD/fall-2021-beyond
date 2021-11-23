using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind.NeuroTags;

/// <summary>
/// From NextMind quick start guide: https://www.next-mind.com/documentation/unity-sdk/tutorials/04-customizing-feedback/
/// </summary>
[RequireComponent(typeof(NeuroTag))]
public class MoveCube : MonoBehaviour
{
    private NeuroTag neuroTag;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Awake()
    {
        neuroTag = this.GetComponent<NeuroTag>();
        neuroTag.onConfidenceChanged.AddListener(OnConfidenceChanged);
    }

    private void Start()
    {
        startPos = transform.Find("Start Position").position;
        endPos = transform.Find("End Position").position;
    }

    private void OnDestroy()
    {
        neuroTag.onConfidenceChanged.RemoveListener(OnConfidenceChanged);
    }

    private void OnConfidenceChanged(float percent)
    {
        transform.position = startPos + percent * (endPos - startPos);
    }
}
