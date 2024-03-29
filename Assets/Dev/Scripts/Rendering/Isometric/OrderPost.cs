using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out OrderedObject obj))
        {
            obj.SetStencilState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out OrderedObject obj))
        {
            obj.SetStencilState(false);
        }
    }
}
