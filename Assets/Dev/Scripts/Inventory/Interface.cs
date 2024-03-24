using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IInventoryModel
{
    
}

public interface IInventroyView
{
    
}

public interface IInventoryBaseController<MODEL, VIEW> 
    where MODEL : IInventoryModel
    where VIEW : IInventroyView
{
    public MODEL Model { get; set; }
    public VIEW View { get; set; }
}