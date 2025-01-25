using System.Collections;
using System.Collections.Generic;
using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.Features;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class MRNPCActionInitializer : MonoBehaviour
{
    private MRUK _mruk;
    private MRUKRoom _mrukRoom;
    private ConvaiInteractablesData _convaiInteractablesData;
    private NavMeshBuilder _navMeshBuilder;

    [SerializeField] MRUKAnchor.SceneLabels _sceneLabels;

    private void Awake()
    {
        _mruk = FindObjectOfType<MRUK>();
        _convaiInteractablesData = FindObjectOfType<ConvaiInteractablesData>();
        _navMeshBuilder = FindObjectOfType<NavMeshBuilder>();
    }

    private void OnEnable()
    {
        _mruk.SceneLoadedEvent.AddListener(MRUK_OnSceneLoadedEvent);
    }

    private void MRUK_OnSceneLoadedEvent()
    {
        _mrukRoom = _mruk.GetCurrentRoom();
        StartCoroutine(InitializeAction(_mrukRoom.Anchors));
    }

    private IEnumerator InitializeAction(List<MRUKAnchor> MRUKAnchors)
    {
        UpdateActionObjects(MRUKAnchors);
        yield return new WaitForSeconds(1f);

        ConvaiNPC[] convaiNPCs = FindObjectsOfType<ConvaiNPC>();

        UpdateActionCharacters(convaiNPCs);

        if (_navMeshBuilder.TryToBuildNavMesh())
        {
            Debug.Log("NavMesh built successfully!");
        }
        else
        {
            Debug.LogError("NavMesh failed to build!");
        }
    }

    private void UpdateActionObjects(List<MRUKAnchor> MRUKAnchors)
    {
        List<ConvaiInteractablesData.Object> objectList = new List<ConvaiInteractablesData.Object>();

        foreach (ConvaiInteractablesData.Object interactableDataObject in _convaiInteractablesData.Objects)
        {
            objectList.Add(interactableDataObject);
        }

        foreach (MRUKAnchor mrukAnchor in MRUKAnchors)
        {
            if (LabelFilter.Included(mrukAnchor.Label).PassesFilter(_sceneLabels))
            {
                ConvaiInteractablesData.Object obj = new ConvaiInteractablesData.Object
                {
                    Name = mrukAnchor.name,
                    Description = "",
                    gameObject = mrukAnchor.gameObject
                };
                objectList.Add(obj);
            }
        }

        _convaiInteractablesData.Objects = objectList.ToArray();
    }

    private void UpdateActionCharacters(ConvaiNPC[] convaiNPCs)
    {
        List<ConvaiInteractablesData.Character> characterList = new List<ConvaiInteractablesData.Character>();

        foreach (ConvaiNPC convaiNPC in convaiNPCs)
        {
            ConvaiInteractablesData.Character character = new ConvaiInteractablesData.Character
            {
                Name = convaiNPC.name,
                Bio = "",
                gameObject = convaiNPC.gameObject
            };

            characterList.Add(character);
        }


        ConvaiInteractablesData.Character player = new ConvaiInteractablesData.Character
        {
            Name = "Player",
            Bio = "Me",
            gameObject = Camera.main.gameObject
        };

        characterList.Add(player);

        _convaiInteractablesData.Characters = characterList.ToArray();
    }
}