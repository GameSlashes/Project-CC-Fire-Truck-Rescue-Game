using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallitOnEnable : MonoBehaviour
{
    public MapLine mapLine;
    private void OnEnable()
    {
        mapLine.startPoint = gameObject;
    }
}
