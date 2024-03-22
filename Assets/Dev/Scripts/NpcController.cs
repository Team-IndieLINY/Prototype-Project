using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private NpcInventory _inventory;
    [SerializeField] private SteminaView _steminaView;
    [SerializeField] private ActorSteminaData _data;
    [SerializeField] private ScriptView _scriptView;
    [SerializeField] private ScriptController _scriptController;
    [SerializeField] private ScriptData _scriptModel;

    [SerializeField] private CollisionInteraction _interaction;
    public CollisionInteraction Interaction => _interaction;
    
    public SteminaController Stemina { get; private set; }

    private void Awake()
    {
        Stemina = new SteminaController(Interaction, _steminaView, _data);
        
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

        _scriptModel = _scriptModel.Clone();
        _scriptController = new ScriptController(_scriptModel, _scriptView, Stemina.Properties, _data);
    }

    private void Update()
    {
        if (Stemina.Properties.GetValue<int>(EStatCode.Health) <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnEaten(SteminaController controller)
    {
        _scriptView.Show(_scriptModel.Eaten);
    }
}
