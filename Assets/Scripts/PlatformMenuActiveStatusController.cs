using Convai.Scripts.Runtime.Core;
using UnityEngine;

public class PlatformMenuActiveStatusController : MonoBehaviour
{
    [SerializeField] private GameObject platformMenu;
    private void OnEnable()
    {
        ConvaiInputManager.Instance.togglePlatformMenu += ConvaiInputManager_TogglePlatformMenu;
    }

    private void OnDisable()
    {
        ConvaiInputManager.Instance.togglePlatformMenu -= ConvaiInputManager_TogglePlatformMenu;
    }

    private void ConvaiInputManager_TogglePlatformMenu()
    {
        platformMenu.SetActive(!platformMenu.activeSelf);
    }
}