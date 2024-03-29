using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public enum EHouseDirection
{
    None,
    Front,
    Back,
    Inside,
    Stair
}

public enum EHouseState
{
    Inside,
    Front,
    Back,
}
public class House : MonoBehaviour
{
    private List<HouseModule> _modules = new ();

    private List<HousePass> _housePasses = new();

    public AsyncReactiveProperty<(EHouseDirection, OrderedActor)> OnPass;

    private EHouseDirection _currentValue;

    public int OutsideBackOrder => -3;
    public int BackOrder => -2;
    public int FloorOrder => -1;
    public int InsideOrder => 0;
    public int FrontOrder => 1;
    public int OutsideFrontOrder => 2;

    public Action _stateCallback;
    public int CurrentFloor = 1;

    private void Awake()
    {
        OnPass = new AsyncReactiveProperty<(EHouseDirection, OrderedActor)>((EHouseDirection.None, null));
        UniTask.Create(async () => OnUpdate());
        _currentValue = EHouseDirection.Front;
        
        foreach (var obj in GetComponentsInChildren<HousePass>())
        {
            _housePasses.Add(obj);
            obj.Init(this);
        }
        
        foreach (var obj in GetComponentsInChildren<HouseModule>(true))
        {
            _modules.Add(obj);
        }
        

        foreach (var module in _modules)
        {
            SetOrder(BackOrder, module.backs);
            SetOrder(FloorOrder, module.floors);
            SetOrder(FrontOrder, module.fronts);
            SetOrder(FrontOrder, module.fronts_collider);
            SetOrder(BackOrder, module.backs_collider);
        }
    }

    private async void OnUpdate()
    {
        while (true)
        {
            var tuple = await OnPass.WaitAsync();

            ModuleUpdate(tuple);
        }
    }
    
    public void ModuleUpdate((EHouseDirection, OrderedActor) tuple)
    {
        var dir = tuple.Item1;
        var orderedObject = tuple.Item2;

        if (orderedObject == null) return;

        if (_currentValue == EHouseDirection.Inside && dir == EHouseDirection.Back)
        {
            return;
        }
        else
        {
            _currentValue = dir;
        }

        switch (dir)
        {
            case EHouseDirection.None:
                break;
            case EHouseDirection.Front:
                if (orderedObject.IsPlayer)
                {
                    _stateCallback = OnFront;
                }

                orderedObject.Order = 2;
                break;
            case EHouseDirection.Back:
                if (orderedObject.IsPlayer)
                {
                    _stateCallback = OnBack;
                }

                orderedObject.Order = -3;
                break;
            case EHouseDirection.Inside:
                if (orderedObject.IsPlayer)
                {
                    _stateCallback = OnInsideOnFloor;
                }

                orderedObject.Order = 0;
                break;
            case EHouseDirection.Stair:
                if (orderedObject.IsPlayer)
                {
                    _stateCallback = OnStair;
                }

                orderedObject.Order = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _stateCallback();
    }

    private void SetEnable(bool value, List<SpriteRenderer> arr)
    {
        foreach (var item in arr)
        {
            item.enabled = value;
        }
    }

    private void SetOrder(int value, List<SpriteRenderer> arr)
    {
        foreach (var item in arr)
        {
            item.sortingOrder = value;
        }
    }

    private void SetAlpha(HouseModule module, float value, bool exclusive1F)
    {
        foreach (var item in module.fronts)
        {
            var c = item.color;
            c.a = value;
            item.color = c;
        }

        foreach (var item in module.backs)
        {
            var c = item.color;
            c.a = value;
            item.color = c;
        }

        foreach (var item in module.floors)
        {
            var c = item.color;
            c.a = value;
            item.color = c;
        }

        if (module.floor != 1 && exclusive1F == true) return;

        foreach (var item in module.fronts_collider)
        {
            var c = item.color;
            c.a = value;
            item.color = c;
        }
        foreach (var item in module.backs_collider)
        {
            var c = item.color;
            c.a = value;
            item.color = c;
        }
    }

    private void OnFront()
    {
        foreach (var module in _modules)
        {
            SetEnable(true, module.fronts);
            SetEnable(true, module.backs);
            SetEnable(true, module.floors);
            SetEnable(true, module.fronts_collider);
            SetEnable(true, module.backs_collider);
                    
            SetAlpha(module, 1f, false);
        }
    }
    private void OnBack()
    {
        foreach (var module in _modules)
        {
            SetEnable(true, module.fronts);
            SetEnable(true, module.backs);
            SetEnable(true, module.floors);
            SetEnable(true, module.fronts_collider);
            SetEnable(true, module.backs_collider);

            SetAlpha(module, 0.5f, false);
        }
        
        foreach (var module in _modules)
        {
            foreach (var col in module.fronts_collider)
            {
                col.GetComponent<Collider2D>().enabled = module.floor == 1;
            }
            foreach (var col in module.backs_collider)
            {
                col.GetComponent<Collider2D>().enabled = module.floor == 1;
            }
        }
    }

    private void OnStair()
    {
        //_backPass.GetComponent<Collider2D>().enabled = false;
        
        foreach (var module in _modules)
        {
            SetEnable(false, module.fronts);
            SetEnable(true, module.backs);
            SetEnable(true, module.floors);
            SetEnable(false, module.fronts_collider);
            SetEnable(true, module.backs_collider);

                    
            SetAlpha(module, 1f, false);
        }

        foreach (var module in _modules)
        {
            foreach (var col in module.backs_collider)
            {
                col.GetComponent<Collider2D>().enabled = false;
            }
            foreach (var col in module.fronts_collider)
            {
                col.GetComponent<Collider2D>().enabled = false;
            }
            foreach (var col in module.fronts)
            {
                col.enabled =  module.floor < CurrentFloor;
            }
            foreach (var col in module.backs)
            {
                col.enabled =  module.floor <= CurrentFloor;
            }
            foreach (var col in module.floors)
            {
                col.enabled =  module.floor <= CurrentFloor;
            }
        }
    }
    
    private void OnInsideOnFloor()
    {
        //_backPass.GetComponent<Collider2D>().enabled = false;
        
        foreach (var module in _modules)
        {
            SetEnable(false, module.fronts);
            SetEnable(true, module.backs);
            SetEnable(true, module.floors);
            SetEnable(false, module.fronts_collider);
            SetEnable(true, module.backs_collider);

                    
            SetAlpha(module, 1f, false);
        }

        foreach (var module in _modules)
        {
            foreach (var col in module.backs_collider)
            {
                col.GetComponent<Collider2D>().enabled = module.floor == CurrentFloor;
                col.enabled =  module.floor <= CurrentFloor;
            }
            foreach (var col in module.fronts_collider)
            {
                col.GetComponent<Collider2D>().enabled = module.floor == CurrentFloor;
                col.enabled = module.floor <= CurrentFloor;
            }
            foreach (var col in module.fronts)
            {
                col.enabled =  module.floor < CurrentFloor;
            }
            foreach (var col in module.backs)
            {
                col.enabled =  module.floor <= CurrentFloor;
            }
            foreach (var col in module.floors)
            {
                col.enabled =  module.floor <= CurrentFloor;
            }
        }
    }
}