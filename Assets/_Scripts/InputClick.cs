using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputClick : NetworkBehaviour
{
    private Camera _mainCamera;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();

    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay((Vector3)Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        Debug.Log("ray hit collider");
        Debug.Log(rayHit.collider.gameObject.name);
    }
}
