using System.Collections;
using UnityEngine;

public class PistonController : MonoBehaviour
{
    /// <summary>
    /// Piston Child Add ~ Drag, Controller etc.
    /// </summary>
    #region Definitions
    [Header(" Settings ")]
    private bool isDragging = false;
    private Rigidbody rb;
    private Vector3 offset;
    private float dragSpeed = 200f;
    private bool controller = false;
    [Header(" Elements ")]
    [Tooltip(" Correct Position Object ")]
    [Space(3)]
    [SerializeField]
    private GameObject correctPositionObject; // Serialized field for correct position GameObject
    [Tooltip(" Correct Sound ")]
    [SerializeField]
    private AudioSource correctSound;
    #endregion
    #region Unity Callbacks

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Check if the object is in the correct position when releasing the mouse
        CheckCorrectPosition();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos();
            Vector3 targetPos = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, mousePos.z + offset.z);

            rb.MovePosition(Vector3.Lerp(transform.position, targetPos, Time.deltaTime * dragSpeed));
        }
    }

    #endregion
    #region Helper Methods
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    private void CheckCorrectPosition()
    {
        if (correctPositionObject == null)
        {
            Debug.LogWarning("Correct position object not assigned!");
            return;
        }

        float threshold = 0.05f;

        if (controller == false)
        {
            float xDistance = Mathf.Abs(transform.position.x - correctPositionObject.transform.position.x);
            float yDistance = Mathf.Abs(transform.position.y - correctPositionObject.transform.position.y);
            float zDistance = Mathf.Abs(transform.position.z - correctPositionObject.transform.position.z);
            if ((xDistance < threshold) && (yDistance < threshold) && (zDistance < threshold))
            {
                Debug.Log("Object is in the correct position!");

                if (gameObject.CompareTag("PistonPart"))
                {
                    var pistonAnimator = gameObject.GetComponent<Animator>();
                    pistonAnimator.SetTrigger("Rotation");
                }

                correctPositionObject.SetActive(false);
                StartCoroutine(MoveToTargetAndSnap(correctPositionObject.transform.position, threshold));
                correctSound.Play();
                controller = true;

                // Count plus
                GameManager.count++;
            }
            // Check if the distance is within the range (0.051 to 0.2) to activate/deactivate the correct position object

            if (xDistance >= 0.2f || yDistance >= 0.2f || zDistance >= 0.2f)
            {
                correctPositionObject.SetActive(false);
            }
            else if ((xDistance > 0.051f && xDistance < 0.2f) || (yDistance > 0.051f && yDistance < 0.2f) || (zDistance > 0.051f && zDistance < 0.2f))
            {
                correctPositionObject.SetActive(true);
            }
        }
        else
        {
            // Play idle animation and decrement count if the object is a "PistonPart"
            if (gameObject.CompareTag("PistonPart"))
            {
                var pistonAnimator = gameObject.GetComponent<Animator>();
                pistonAnimator.Play("Idle");
            }

            GameManager.count--;
            controller = false;
            correctPositionObject.SetActive(true);
        }
    }
    #endregion
    #region Move To Target Slow
    private IEnumerator MoveToTargetAndSnap(Vector3 targetPosition, float threshold)
    {
        float elapsedTime = 0f;
        float duration = 1.0f;

        Vector3 initialPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
}
