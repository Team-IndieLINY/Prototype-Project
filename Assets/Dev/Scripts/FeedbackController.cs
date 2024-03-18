using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EFeedbackType
{
    Food,
}
public class FeedbackController : MonoBehaviour
{
    [SerializeField] private FeedbackData _feedbackData;
    [SerializeField] private GameObject _masterView;
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _masterView.SetActive(false);
    }

    public void AnimateFeedback(EFeedbackType type)
    {
        StopAllCoroutines();

        _masterView.SetActive(true);
        StartCoroutine(CoAnimateFeedback(type));
    }

    IEnumerator CoAnimateFeedback(EFeedbackType type)
    {
        switch (type)
        {
            case EFeedbackType.Food:
                _text.text = "yummy";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        yield return new WaitForSeconds(1.5f);
        _masterView.SetActive(false);
    }
}
