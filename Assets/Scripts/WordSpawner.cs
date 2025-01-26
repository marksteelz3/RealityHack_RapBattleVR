using System.Collections;
using System.Collections.Generic;
using Convai.Scripts.Runtime.Core;
using GLTFast.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    public ConvaiGRPCAPI voiceApiRef;
    public GameObject wordsPrefab;
    private string lastHumanSpeech;

    private void Update()
    {
        if (ShouldSpawn())
        {
            Spawnwords();

        }


    }

    private void Spawnwords()
    {
        GameObject spawnedWord = Instantiate(wordsPrefab);
        //To do List:
        //Random Placement of SpawnedWord
        //Spawning multiple objects from one HumanRap
        //Parent SpawnedWord to "RoboRapper" to have text move with target
        //Randomized Scale and font
        //Set Minimum and maximum Range
        //Keyword Splitter
        //Spawn in Key words

        Wordprefabcell wordPrefabScript = spawnedWord.GetComponent<Wordprefabcell>();
        wordPrefabScript.SetText(voiceApiRef.HumanRap);


    }

    private bool ShouldSpawn()
    {
        if (lastHumanSpeech != voiceApiRef.HumanRap && lastHumanSpeech !=  string.Empty)
        { 
            lastHumanSpeech = voiceApiRef.HumanRap;
            return true;

        }
        return false;
    }


}
