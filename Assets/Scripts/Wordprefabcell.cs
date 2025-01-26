using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wordprefabcell : MonoBehaviour
{
    public TextMeshPro textField;

    public void SetText(string newText)
    { 
        textField.SetText(newText);
    
    }
}
