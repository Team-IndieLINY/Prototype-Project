using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SteminaView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public float Health { get; set; }
    public float Food { get; set; }


    public void UpdateView()
    {
        Debug.Assert(_text);
        
        _text.text = $@"Health: {Health}
Food: {Food}"
                      ;
    }
}
