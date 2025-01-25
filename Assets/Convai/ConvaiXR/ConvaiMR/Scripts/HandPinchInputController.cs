using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.UI;
using UnityEngine;

public class HandPinchInputController : MonoBehaviour
{
    [SerializeField] private OVRHand _talkHand;
    [SerializeField] private OVRHand _settingsPanelHand;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private float _pinchThreshold = 0.85f;

    private bool _isPinchingTalkHand = false;
    private bool _isSettingsPanelActive = false;
    private ConvaiNPC _currentActiveNPC;

    private void OnEnable()
    {
        ConvaiNPCManager.Instance.OnActiveNPCChanged += ConvaiNPCManager_OnActiveNPCChanged;
    }

    private void OnDisable()
    {
        ConvaiNPCManager.Instance.OnActiveNPCChanged -= ConvaiNPCManager_OnActiveNPCChanged;
    }

    private void ConvaiNPCManager_OnActiveNPCChanged(ConvaiNPC newConvaiNPC)
    {
        if (newConvaiNPC == null) return;

        _currentActiveNPC = newConvaiNPC;
    }

    private void Update()
    {
        HandleTalkHand();
        HandleSettingsPanelHand();
    }

    private void HandleTalkHand()
    {
        if (_currentActiveNPC == null) return;
        
        if (_isSettingsPanelActive) return;

        bool currentlyPinching = HasPinched(_talkHand);

        if (currentlyPinching)
        {
            if (!_isPinchingTalkHand)
            {
                Debug.Log("Talk hand pinched");
                _isPinchingTalkHand = true;
                HandleVoiceListening(true);
            }
        }
        else
        {
            if (_isPinchingTalkHand)
            {
                Debug.Log("Talk hand unpinched");
                _isPinchingTalkHand = false;
                HandleVoiceListening(false);
            }
        }
    }

    private void HandleSettingsPanelHand()
    {
        bool currentlyPinching = HasPinched(_settingsPanelHand);
        _isSettingsPanelActive = _settingsPanel.gameObject.activeSelf;

        if (currentlyPinching && !_isSettingsPanelActive)
        {
            ConvaiInputManager.Instance.toggleSettings?.Invoke();
        }
    }

    private bool HasPinched(OVRHand hand)
    {
        OVRHand.HandFinger finger = OVRHand.HandFinger.Index;
        
        bool isIndexFingerPinching = hand.GetFingerIsPinching(finger);
        OVRHand.TrackingConfidence trackingConfidence = hand.GetFingerConfidence(finger);
        
        float pinchStrength = hand.GetFingerPinchStrength(finger);
        
        return isIndexFingerPinching && trackingConfidence == OVRHand.TrackingConfidence.High && pinchStrength >= _pinchThreshold;
    }

    private void HandleVoiceListening(bool listenState)
    {
        if (UIUtilities.IsAnyInputFieldFocused() || !_currentActiveNPC.isCharacterActive) return;
        switch (listenState)
        {
            case true:
                _currentActiveNPC.InterruptCharacterSpeech();
                _currentActiveNPC.playerInteractionManager.UpdateActionConfig();
                _currentActiveNPC.StartListening();
                break;
            case false:
            {
                if (_currentActiveNPC.isCharacterActive) _currentActiveNPC.StopListening();
                break;
            }
        }
    }
}