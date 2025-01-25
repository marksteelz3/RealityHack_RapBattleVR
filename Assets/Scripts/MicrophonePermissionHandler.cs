using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
public class MicrophonePermissionHandler : MonoBehaviour
{
    private void Awake()
    {
        RequestMicrophonePermissions();
    }

    /// <summary>
    ///     Request Microphone permissions on Android or iOS.
    /// </summary>
    private void RequestMicrophonePermissions()
    {
#if UNITY_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
        }
        else
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
}