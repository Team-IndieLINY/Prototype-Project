using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ScriptEntity
{
    [Header("Display")]
    [SerializeField] private string _text;
    [SerializeField] private float _displayDuration = 1.5f;
    [SerializeField] private float _coolTime = 10f;

    [FormerlySerializedAs("healthPercentage")]
    [Header("Conditions")]
    [SerializeField] private float _healthPercentage = 0;
    [SerializeField] private float _foodPercentage = 0;
    [SerializeField] private float _thristyPercentage = 0;

    public string Text => _text;

    public float HealthPercentage => _healthPercentage;

    public float FoodPercentage => _foodPercentage;

    public float ThristyPercentage => _thristyPercentage;

    public float DisplayDuration => _displayDuration;

    public float CoolTime => _coolTime;

    public bool CanDisplay { get; set; }

    public ScriptEntity(ScriptEntity entity)
    {
        _text = entity._text;
        _displayDuration = entity._displayDuration;
        _coolTime = entity._coolTime;
        this._healthPercentage = entity._healthPercentage;
        this._foodPercentage = entity._foodPercentage;
        this._thristyPercentage = entity._thristyPercentage;
        this.CanDisplay = entity.CanDisplay;

        CanDisplay = true;
    }
}

[CreateAssetMenu(menuName = "IndieLINY/Script", fileName = "Script")]
public class ScriptData : ScriptableObject
{
    [SerializeField] private List<ScriptEntity> _entities;

    [SerializeField] private ScriptEntity _eaten;

    public List<ScriptEntity> Entities => _entities;
    public ScriptEntity Eaten => _eaten;

    public ScriptData Clone()
    {
        var data = ScriptableObject.CreateInstance<ScriptData>();

        data._entities = _entities.Select(x => new ScriptEntity(x)).ToList();
        data._eaten = new ScriptEntity(_eaten);
        
        return data;
    }
}
