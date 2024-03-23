using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;


public class SteminaController : MonoBehaviour, IBActorStemina
{
    [SerializeField] private CollisionInteraction _interaction;
    [SerializeField] private SteminaView _view;
    [SerializeField] private ValueUpdaterFactory _factory;
    
    public SteminaView View => _view;
    public CollisionInteraction Interaction => _interaction;
    public PropertiesConatiner<StatDataValue, int> StatProperties { get; private set; }
    
    public event Action<SteminaController> OnEaten;
    

    public bool Enabled = true;

    private StatTable _table;

    private void Awake()
    {
        _table = TableContainer.Instance.Get<StatTable>("Stat");
        
        StatProperties = StatUtils.CreateProperties(1, _factory, _table);
        StatUtils.UpdateValidation(this, _table);
        StatUtils.UpdateView(this);
    }

    private void Update()
    {
        StatUtils.FrameUpdateStemina(this, _table);
        StatUtils.UpdateValidation(this, _table);
        StatUtils.UpdateView(this);
    }

    public void Eat(Item item)
    {
        //TODO: 음식 먹었을 때 스텟 상태 갱신 코드 작성

        StatUtils.UpdateValidation(this, _table);
        StatUtils.UpdateView(this);

        OnEaten?.Invoke(this);
    }

    private void OnDestroy()
    {
        StatUtils.Release(this);
    }
}