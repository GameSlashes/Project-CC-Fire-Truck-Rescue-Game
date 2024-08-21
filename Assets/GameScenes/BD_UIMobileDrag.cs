using UnityEngine;
using UnityEngine.EventSystems;
public class BD_UIMobileDrag : MonoBehaviour, IDragHandler, IEndDragHandler {

    private BD_ShowroomCamera showroomCamera;

    private void Awake() {

        showroomCamera = FindObjectOfType<BD_ShowroomCamera>();

    }

    public void OnDrag(PointerEventData data) {

        if (showroomCamera)
            showroomCamera.OnDrag(data);

    }

    public void OnEndDrag(PointerEventData data) 
    {
    }

}
