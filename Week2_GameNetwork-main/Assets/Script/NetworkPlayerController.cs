using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedGravity = -2f;

    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float jumpForce = 8f;

    private float xRotation;
    private float yRotation;

    private CharacterController characterController;
    private float verticalVelocity;

    private Vector2 moveInput;
    private bool jumpInput;
    private float mouseXInput;
    private float mouseYInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        playerCamera.gameObject.SetActive(IsOwner);

        if (IsOwner)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        HandleInput();

        SubmitInputServerRpc(moveInput, jumpInput, mouseXInput, mouseYInput);

        jumpInput = false;
    }

    private void HandleInput()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }

        xRotation -= mouseYInput * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    [ServerRpc]
    private void SubmitInputServerRpc(Vector2 input, bool jump, float mouseX, float mouseY)
    {
        moveInput = input;
        jumpInput = jump;

        transform.Rotate(Vector3.up * mouseX * mouseSensitivity * Time.deltaTime);

        MoveOnServer();
    }

    private void MoveOnServer()
    {
        if (characterController.isGrounded)
        {
            if (jumpInput)
                verticalVelocity = jumpForce;
            else if (verticalVelocity < 0)
                verticalVelocity = groundedGravity;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 moveDirection =
            (transform.right * moveInput.x +
             transform.forward * moveInput.y).normalized;

        Vector3 movement =
            moveDirection * moveSpeed +
            Vector3.up * verticalVelocity;

        characterController.Move(movement * Time.deltaTime);
    }
}