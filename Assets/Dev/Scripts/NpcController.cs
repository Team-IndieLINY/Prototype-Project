using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private NpcInventory _inventory;
    [SerializeField] private ActorSteminaData _steminaData;
    [SerializeField] private SteminaView _steminaView;
    [SerializeField] private FeedbackController _feedbackController;

    [SerializeField] private CollisionInteraction _interaction;
    public CollisionInteraction Interaction => _interaction;
    
    public SteminaController Stemina { get; private set; }

    private void Awake()
    {
        Stemina = new SteminaController(Interaction, _steminaData, _steminaView);
        
        Interaction.SetContractInfo(ActorContractInfo.Create(
            transform,
            () => gameObject == false)
        );

        if (Interaction.ContractInfo is ActorContractInfo info)
        {
            info
                .AddBehaivour<IBActorStemina>(Stemina)
                ;
        }

        StartCoroutine(Stemina.UpdatePerSec());

        Stemina.OnEaten += OnEaten;
    }

    private void Update()
    {
        if (Stemina.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnEaten(SteminaController controller)
    {
        _feedbackController.AnimateFeedback(EFeedbackType.Food);
    }
}
