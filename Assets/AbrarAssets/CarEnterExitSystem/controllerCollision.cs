using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerCollision : MonoBehaviour
{
    public GameObject carEngineEnabled;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Water")
        {
            GameManager.instance._controllerDetection.getOutBtn();
            gameObject.SetActive(false);
            Destroy(transform.parent.gameObject, 10f);
        }
    }
}
