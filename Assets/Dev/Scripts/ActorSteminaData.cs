using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "IndieLINY/Stemina", fileName = "Stemina", order = 3)]
public class ActorSteminaData : ScriptableObject
{
    [SerializeField] private float _maxFood;
    [SerializeField] private float _maxHealth;

    [SerializeField] private float _decreaseFoodPerSec;
    [SerializeField] private float _decreaseHealthWhenNotEnoughFoodPerSec;

    
    public float DecreaseFoodPerSec => _decreaseFoodPerSec;
    public float DecreaseHealthWhenNotEnoughFoodPerSec => _decreaseHealthWhenNotEnoughFoodPerSec;
    public float MaxFood => _maxFood;
    public float MaxHealth => _maxHealth;
}
