using UnityEngine;
using UnityEngine.EventSystems;
public class BD_ShowroomCamera : MonoBehaviour {

    public Transform target; 
    public float distance = 8f;  
    [Space]
    public bool orbitingNow = true;  
    public float orbitSpeed = 5f;     
    [Space]
    public bool smooth = true;
    public float smoothingFactor = 5f;
    [Space]
    public float minY = 5f;
    public float maxY = 35f;
    [Space]
    public float dragSpeed = 10f;
    public float orbitX = 0f;
    public float orbitY = 0f;

    private void LateUpdate() {

        if (!target) {

            Debug.LogWarning("Camera target not found!");
            enabled = false;
            return;

        }

        if (orbitingNow)
            orbitX += Time.deltaTime * orbitSpeed;

        orbitY = ClampAngle(orbitY, minY, maxY);

        Quaternion rotation = Quaternion.Euler(orbitY, orbitX, 0);
        Vector3 position = rotation * new Vector3(0f, 0f, -distance) + target.transform.position;

        if (!smooth)
            transform.SetPositionAndRotation(position, rotation);
        else
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, position, Time.unscaledDeltaTime * 10f), Quaternion.Slerp(transform.rotation, rotation, Time.unscaledDeltaTime * 10f));

    }

    private float ClampAngle(float angle, float min, float max) {

        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);

    }

    public void ToggleAutoRotation(bool state) {

        orbitingNow = state;

    }

    public void OnDrag(PointerEventData pointerData) {

        orbitX += pointerData.delta.x * dragSpeed * .02f;
        orbitY -= pointerData.delta.y * dragSpeed * .02f;

    }

    public void SetHorizontal(float hor)
    {

        orbitX = hor;

    }
    /// <summary>
    /// Sets vertical angle of the showroom camera.
    /// </summary>
    /// <param name="ver"></param>
    public void SetVertical(float ver)
    {

        orbitY = ver;

    }

}
