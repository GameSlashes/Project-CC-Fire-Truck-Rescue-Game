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

    private void OnEnable()
    {
        navigationSystem.PlayerCamera = playerCameraComponent.GetComponent<Camera>();
        navigationSystem.PlayerController = playerControllerComponent.transform;
        minimapRenderer.minimapCameraToShow = minimapCamera;
    }
}
