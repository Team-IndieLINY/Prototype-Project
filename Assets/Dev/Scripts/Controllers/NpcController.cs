using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private NpcInventory _inventory;
    
    [SerializeField] private ScriptView _scriptView;
    [SerializeField] private ScriptController _scriptController;
    [SerializeField] private ScriptData _scriptModel;
    [SerializeField] private GameObject _tomb;

    [SerializeField] private CollisionInteraction _interaction;
    [SerializeField] private SteminaController _steminaController;
    
    public CollisionInteraction Interaction => _interaction;
    public SteminaController Stemina => _steminaController;

    private void Awake()
    {
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

        Stemina.OnEaten += OnEaten;

        _scriptModel = _scriptModel.Clone();
        _scriptController = new ScriptController(_scriptModel, _scriptView, Stemina.StatProperties);
    }

    private void Update()
    {
        //TODO: 체력이 0 일 때 처리
    }

    private void OnDestroy()
    {
        var obj = Instantiate(_tomb);
        obj.SetActive(true);
        obj.transform.position = transform.position;
    }

    private void OnEaten(SteminaController controller)
    {
        if (TryGetComponent(out AudioSource source))
        {
            source.Play();
        }
        _scriptView.Show(_scriptModel.Eaten);
    }
}
