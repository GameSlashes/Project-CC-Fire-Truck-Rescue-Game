using UnityEngine;
using System.Collections;

public class DestroyGameObject : MonoBehaviour
{

    public float clearDistance = 150.0f;
    public GameObject myRoot;
    public Renderer myBody;
    void Update()
    {

        if (!CreateAI.Instance.player) return;

        if (Vector3.Distance(transform.position, CreateAI.Instance.player.transform.position) > clearDistance && !myBody.isVisible)
        {
            Destroy(myRoot);
            CreateAI.Instance.currentHumans--;
        }

    }
}
