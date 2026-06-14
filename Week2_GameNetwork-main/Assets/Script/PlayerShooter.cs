using Unity.Netcode;
using UnityEngine;

public class PlayerShooter : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float fireRate = 0.3f;
    private float nextFireTime;

    public override void OnNetworkSpawn()
    {
        nextFireTime = 0f;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (GameManager.Instance.timer.Value <= 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Debug.Log("Mouse Clicked");
            nextFireTime = Time.time + fireRate;
            ShootServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void ShootServerRpc()
    {
        Debug.Log("Shot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired\nShot fired");

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                GameManager.Instance.AddScore(OwnerClientId);
            }
        }
    }
}