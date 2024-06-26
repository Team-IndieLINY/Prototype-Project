using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteminaView : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _foodBar;
    [SerializeField] private Image _thirstyBar;
    [SerializeField] private Image _temperatureBar;

    public int MaxHealth { get; set; }
    public int Health { get; set; }
    
    public int MaxFood { get; set; }
    public int Food { get; set; }
    
    public int MaxThirsty { get; set; }
    public int Thirsty { get; set; }
    
    public int MaxTemperature { get; set; }
    public int Temperature { get; set; }


    public void UpdateView()
    {
        _healthBar.fillAmount = (float)Health / MaxHealth;
        _foodBar.fillAmount = (float)Food / MaxFood;
        _thirstyBar.fillAmount = (float)Thirsty / MaxThirsty;
        _temperatureBar.fillAmount = (float)Temperature / MaxTemperature;
    }
}
