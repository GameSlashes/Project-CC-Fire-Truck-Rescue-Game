using UnityEngine;
using SickscoreGames.HUDNavigationSystem;
using MTAssets.EasyMinimapSystem;

public class HUDEnableData : MonoBehaviour
{
    public HUDNavigationSystem navigationSystem;
    public HNSPlayerCamera playerCameraComponent;
    public HNSPlayerController playerControllerComponent;

    public MinimapCamera minimapCamera;
    public MinimapRenderer minimapRenderer;
    public RTC_TrafficSpawner cam;

    private void OnEnable()
    {
        navigationSystem.PlayerCamera = playerCameraComponent.GetComponent<Camera>();
        navigationSystem.PlayerController = playerControllerComponent.transform;
        minimapRenderer.minimapCameraToShow = minimapCamera;

        cam.transform.SetParent(gameObject.transform);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;
    }
}
