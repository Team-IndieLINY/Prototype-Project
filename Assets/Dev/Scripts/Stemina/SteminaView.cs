using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SteminaView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public int Health { get; set; }
    public int Food { get; set; }
    public int Thirsty { get; set; }
    public int Temperature { get; set; }


    public void UpdateView()
    {
        Debug.Assert(_text);

        _text.text = $@"Health: {Health}
Food: {Food}
Thirsty: {Thirsty}"
                      ;
    }
}
