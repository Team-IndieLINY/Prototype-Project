using System.Collections;
using System.Collections.Generic;
using IndieLINY.Singleton;
using UnityEngine;
using XRProject.Utils.Log;

[Singleton(ESingletonType.Global, 0)]
public class TableContainer : MonoBehaviourSingleton<TableContainer>
{
    public StatTable StatTable { get; private set; }
    public override void PostInitialize()
    {
        var statTable = Resources.Load<StatTable>("Data/StatTable");
        StatTable = statTable;
        
        Debug.Assert(StatTable);
    }
    public override void PostRelease()
    {
        StatTable = null;
    }
}
