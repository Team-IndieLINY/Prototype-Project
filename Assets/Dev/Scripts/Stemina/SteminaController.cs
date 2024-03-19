using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;


public class SteminaController : IBActorStemina
{
    public CollisionInteraction Interaction { get; private set; }
    public SteminaProperties Properties { get; private set; }
    public SteminaView View;

    public event Action<SteminaController> OnEaten;

    public bool Enabled;

    private StatTable _table;
    public SteminaController(CollisionInteraction interaction, SteminaView view)
    {
        _table = TableContainer.Instance.StatTable;
        
        Debug.Assert(interaction);
        Debug.Assert(view);
        
        this.Interaction = interaction;
        Properties = new SteminaProperties();
        this.View = view;
        
        Enabled = true;

        SteminaUtils.SetBasicValue(this, _table);
        SteminaUtils.UpdateValidation(this, _table);
        SteminaUtils.UpdateView(this);
    }

    public IEnumerator UpdatePerSec()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            SteminaUtils.UpdateStemina(this);
            SteminaUtils.UpdateValidation(this, _table);
            
            SteminaUtils.UpdateView(this);
            
            yield return wait;
        }
    }

    public void Eat(Item item)
    {
        Properties.DoAction<int>(EStatCode.Food, x => x + item.FillFood);
        Properties.DoAction<int>(EStatCode.Health, x => x + item.FillHealth);

        SteminaUtils.UpdateValidation(this, _table);
        SteminaUtils.UpdateView(this);

        OnEaten?.Invoke(this);
    }
}
