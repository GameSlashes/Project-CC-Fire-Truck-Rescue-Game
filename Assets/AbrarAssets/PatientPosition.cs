using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientPosition : MonoBehaviour
{
    public static PatientPosition Instance;
    public bool is1;
    public bool is2;
    public bool is3;
    [SerializeField] public Vector3 pos;
    [SerializeField] public Vector3 Rotate;
    public void Start()
    {
        Instance = this;
    }
}
