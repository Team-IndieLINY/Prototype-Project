using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;

public abstract class MaximumValueT<T> where T : struct
{
    private T _value;
    public T MaxValue;

    public MaximumValueT()
    {
    }
    
    public MaximumValueT(T maxValue, T currentValue)
    {
        MaxValue = maxValue;
        Value = currentValue;
    }

    public T Value
    {
        get => _value;
        set
        {
            _value = value;

            if (IsGreaterThen(_value, MaxValue))
            {
                _value = MaxValue;
            }
        }
    }

    protected abstract bool IsGreaterThen(T source, T target);
}

public class MVFloat : MaximumValueT<float>
{
    public MVFloat(float maxValue, float currentValue):base(maxValue, currentValue) {}
    protected override bool IsGreaterThen(float source, float target)
    {
        return source > target;
    }
}

public class SteminaController : IBActorStemina
{
    public CollisionInteraction Interaction { get; private set; }
    public ActorSteminaData SteminaData { get; private set; }
    private SteminaView _view;

    public event Action<SteminaController> OnEaten;

    private MVFloat _food;

    public bool Enabled;
    public float Food
    {
        get => _food.Value;
        set => _food.Value = value;
    }

    private MVFloat _health;
    public float Health
    {
        get => _health.Value;
        set => _health.Value = value;
    }
    
    public SteminaController(CollisionInteraction interaction, ActorSteminaData data, SteminaView view)
    {
        Debug.Assert(interaction);
        Debug.Assert(data);
        Debug.Assert(view);
        
        this.Interaction = interaction;
        this.SteminaData = data;
        this._view = view;

        _food = new MVFloat(data.MaxFood, 0f);
        _health = new MVFloat(data.MaxHealth, 0f);

        SetMaxStemina();

        Enabled = true;
    }

    public IEnumerator UpdatePerSec()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            if (Enabled)
            {
                Food -= SteminaData.DecreaseFoodPerSec;

                if (Food <= 0f)
                {
                    Health -= SteminaData.DecreaseHealthWhenNotEnoughFoodPerSec;
                }
                
                UpdateView();
            }

            yield return wait;
        }
    }

    public void SetMaxStemina()
    {
        Food = SteminaData.MaxFood;
        Health = SteminaData.MaxHealth;

        UpdateView();
    }


    public void Eat(Item item)
    {
        Food += item.FillFood;
        Health += item.FillHealth;

        UpdateView();

        OnEaten?.Invoke(this);
    }

    private void UpdateView()
    {
        _view.Food = Food;
        _view.Health = Health;
        
        _view.UpdateView();
    }
}
