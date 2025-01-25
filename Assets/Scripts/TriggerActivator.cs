using Convai.Scripts.Runtime.Core;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] private GameObject _triggerObject;
  
    public void ActivateTrigger()
    {
        _triggerObject.SetActive(true);
    }
}
