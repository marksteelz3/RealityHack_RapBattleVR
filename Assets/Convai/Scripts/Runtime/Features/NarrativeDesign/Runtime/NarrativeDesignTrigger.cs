using System.Collections.Generic;
using System.Linq;
using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Convai.Scripts.Runtime.Features
{
    public class NarrativeDesignTrigger : MonoBehaviour
    {
        public ConvaiNPC convaiNPC;
        [HideInInspector] public int selectedTriggerIndex;
        [HideInInspector] public List<string> availableTriggers = new();
        public UnityEvent onTriggerEvent;

        private NarrativeDesignManager _narrativeDesignManager;
        private UIAppearanceSettings _uiAppearanceSettings;

        [SerializeField] private bool _canShowUIAfterTrigger = true;

        private void Awake()
        {
            UpdateNarrativeDesignManager();

            _uiAppearanceSettings = FindObjectOfType<UIAppearanceSettings>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter" + other.gameObject.name);
            if (other.gameObject.CompareTag("Player")) InvokeSelectedTrigger();
        }

        private void OnValidate()
        {
            UpdateNarrativeDesignManager();
        }

        private void UpdateNarrativeDesignManager()
        {
            if (convaiNPC != null)
            {
                _narrativeDesignManager = convaiNPC.GetComponent<NarrativeDesignManager>();
                if (_narrativeDesignManager != null) UpdateAvailableTriggers();
            }
            else
            {
                availableTriggers.Clear();
                selectedTriggerIndex = -1;
            }
        }

        public void UpdateAvailableTriggers()
        {
            if (_narrativeDesignManager != null)
            {
                availableTriggers = _narrativeDesignManager.triggerDataList.Select(trigger => trigger.triggerName).ToList();
                if (selectedTriggerIndex >= availableTriggers.Count) selectedTriggerIndex = availableTriggers.Count - 1;
            }
        }

        public void InvokeSelectedTrigger()
        {
            if (convaiNPC != null && availableTriggers != null && selectedTriggerIndex >= 0 && selectedTriggerIndex < availableTriggers.Count)
            {
                string selectedTriggerName = availableTriggers[selectedTriggerIndex];
                if (ConvaiNPCManager.Instance.activeConvaiNPC != convaiNPC)
                {
                    ConvaiNPCManager.Instance.SetActiveConvaiNPC(convaiNPC, false);
                }

                if (_uiAppearanceSettings != null && _canShowUIAfterTrigger)
                {
                    _uiAppearanceSettings.ShowUIAfterTempHide();
                }

                onTriggerEvent?.Invoke();
                convaiNPC.TriggerEvent(selectedTriggerName);
            }
        }
        
        public void InvokeSpeech(string message)
        {
            if (convaiNPC != null && !string.IsNullOrEmpty(message))
            {
                ConvaiNPCManager.Instance.SetActiveConvaiNPC(convaiNPC, false);
                onTriggerEvent?.Invoke();
                convaiNPC.TriggerSpeech(message);
            }
        }

    }
}