using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptView : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _text;
    
    public void Show(ScriptEntity entity)
    {
        StopAllCoroutines();

        _panel.SetActive(true);
        StartCoroutine(CoAnimateFeedback(entity));
    }
    private void Awake()
    {
        _panel.SetActive(false);
    }

    IEnumerator CoAnimateFeedback(ScriptEntity entity)
    {
        _text.text = entity.Text;
        
        yield return new WaitForSeconds(entity.DisplayDuration);
        _panel.SetActive(false);
    }
}
