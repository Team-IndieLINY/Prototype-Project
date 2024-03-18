using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;

[System.Serializable]
public abstract class RangedValueT<T> where T : struct
{
    [SerializeField]
    private T _value;
    public T MaxValue;
    public T MinValue;

    public RangedValueT()
    {
    }
    
    public RangedValueT(T maxValue, T minValue, T currentValue)
    {
        MaxValue = maxValue;
        MinValue = minValue;
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
            else if (IsLessThen(_value, MinValue))
            {
                _value = MinValue;
            }
        }
    }

    protected abstract bool IsGreaterThen(T source, T target);
    protected abstract bool IsLessThen(T source, T target);
}

public class RVFloat : RangedValueT<float>
{
    public RVFloat(float maxValue, float minValue, float currentValue):base(maxValue, minValue, currentValue) {}
    protected override bool IsGreaterThen(float source, float target)
    {
        return source > target;
    }

    protected override bool IsLessThen(float source, float target)
    {
        return source < target;
    }
}

public class SteminaController : IBActorStemina
{
    public CollisionInteraction Interaction { get; private set; }
    public ActorSteminaData SteminaData { get; private set; }
    private SteminaView _view;

    public event Action<SteminaController> OnEaten;

    [SerializeField]
    private RVFloat _food;

    public bool Enabled;
    public float Food
    {
        get => _food.Value;
        set => _food.Value = value;
    }

    [SerializeField]
    private RVFloat _health;
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

        _food = new RVFloat(data.MaxFood, 0f, 0f);
        _health = new RVFloat(data.MaxHealth, 0f, 0f);

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
