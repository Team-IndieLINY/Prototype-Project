using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "IndieLINY/Stemina", fileName = "Stemina", order = 3)]
public class ActorSteminaData : ScriptableObject
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxFood;
    [SerializeField] private int _decreaseFoodPerSec;
    [SerializeField] private int _decreaseHealthWhenNotEnoughFoodPerSec;
    
    
    [SerializeField] private int _maxThristy;
    [SerializeField] private int _decreaseThristyPerSec;
    [SerializeField] private int _decreaseHealthWhenNotEnoughThirstyPerSec;
    
    [SerializeField] private int _maxTemperature;
    [SerializeField] private int _minTemperature;
    [SerializeField] private int _decreaseTemperaturePerSec;
    [SerializeField] private int _increaseTemperaturePerSec;


    [SerializeField] private int _decreaseHealthWhenNotEnoughTemperaturePerSec;
    [SerializeField] private float _BeginIncreaseTemperatureRadius;

    [SerializeField] private float _maxSprintStemina;
    [SerializeField] private float _decraseSprintSteminaPerSec;
    [SerializeField] private float _increaseSprintSteminaPerSec;

    public float IncreaseSprintSteminaPerSec => _increaseSprintSteminaPerSec;

    public float MaxSprintStemina => _maxSprintStemina;


    public float BeginIncreaseTemperatureRadius => _BeginIncreaseTemperatureRadius;

    public int IncreaseTemperaturePerSec => _increaseTemperaturePerSec;

    public int MaxHealth => _maxHealth;

    public int MaxFood => _maxFood;

    public int DecreaseFoodPerSec => _decreaseFoodPerSec;

    public int DecreaseHealthWhenNotEnoughFoodPerSec => _decreaseHealthWhenNotEnoughFoodPerSec;

    public int MaxThristy => _maxThristy;

    public int DecreaseThristyPerSec => _decreaseThristyPerSec;

    public int DecreaseHealthWhenNotEnoughThirstyPerSec => _decreaseHealthWhenNotEnoughThirstyPerSec;

    public int MaxTemperature => _maxTemperature;

    public int MinTemperature => _minTemperature;

    public int DecreaseTemperaturePerSec => _decreaseTemperaturePerSec;

    public int DecreaseHealthWhenNotEnoughTemperaturePerSec => _decreaseHealthWhenNotEnoughTemperaturePerSec;

    public float DecraseSprintSteminaPerSec => _decraseSprintSteminaPerSec;
}
