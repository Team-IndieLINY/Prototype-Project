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

    public int MaxHealth { get; set; }
    public int Health { get; set; }
    
    public int MaxFood { get; set; }
    public int Food { get; set; }
    
    public int MaxThirsty { get; set; }
    public int Thirsty { get; set; }


    public void UpdateView()
    {
        if(MaxHealth != 0) _healthBar.fillAmount = (float)Health / MaxHealth;
        if(MaxFood != 0) _foodBar.fillAmount = (float)Food / MaxFood;
        if(MaxThirsty != 0) _thirstyBar.fillAmount = (float)Thirsty / MaxThirsty;
    }
}
