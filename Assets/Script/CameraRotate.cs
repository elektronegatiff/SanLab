using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Definitions
    [Header(" SerializeField ")]
    [Space(3)]
    [Tooltip(" Target ")]
    [SerializeField] Transform target; // The target point around which the camera rotates
    [Space(3)]
    [Tooltip(" Rotation Speed ")]
    [SerializeField] float rotationSpeed = 5f;
    [Space(3)]
    [Tooltip(" Move Speed ")]
    [SerializeField] float moveSpeed = 5f;
    #endregion
    #region Unity Metods
    void Update()
    {
        // Get input for movement
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera's forward and right vectors
        Vector3 moveDirection = (transform.forward * verticalMovement + transform.right * horizontalMovement).normalized;

        // Move the camera based on input
        MoveCamera(moveDirection);

        // Check for right mouse button hold
        if (Input.GetMouseButton(1))
        {
            // Get input for rotation
            float mouseX = Input.GetAxis("Mouse X");

            // Rotate the camera around the target point based on mouse movement
            RotateCamera(mouseX);
        }
    }
    #endregion
    #region Move Camera
    void MoveCamera(Vector3 moveDirection)
    {
        // Translate the camera position based on the input and move speed//
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
    #endregion
    #region Rotate Camera
    void RotateCamera(float mouseX)
    {
        // Rotate the camera around the target point based on mouse movement
        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed);
    }
    #endregion
}
