using System;
using System.Collections;
using Convai.Scripts.Runtime.Features;
using UnityEngine;

public class NarrativeDesignTourController : MonoBehaviour
{
    [SerializeField] private NarrativeDesignTrigger _welcomeTrigger;
    private ConvaiActionsHandler _convaiActionsHandler;

    private void Awake()
    {
        _convaiActionsHandler = GetComponent<ConvaiActionsHandler>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        _welcomeTrigger.InvokeSelectedTrigger();
    }

    public void MoveToTargetPoint(GameObject targetPoint)
    {
        StartCoroutine(_convaiActionsHandler.MoveTo(targetPoint));
    }
}