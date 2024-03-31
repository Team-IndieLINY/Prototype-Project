using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector
{
    private static ItemSelector instance;
    public ItemDefinition SelectingItem { get; private set; } = null;

    public static ItemSelector Instance
    {
        get
        {
            if(null == instance)
            {
                //게임 인스턴스가 없다면 하나 생성해서 넣어준다.
                instance = new ItemSelector();
            }
            return instance;
        }
    }

    public void SetSelectingItem(ItemDefinition itemDefinition)
    {
        SelectingItem = itemDefinition;
    }
}
