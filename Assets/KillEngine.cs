using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEngine : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        gameObject.GetComponent<RCCP_CarController>().enabled = false;
    }
}
