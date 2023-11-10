using UnityEngine;

public class PistonController : MonoBehaviour
{
    #region Definitions
    [Header(" Settings ")]
    private bool isDragging = false;
    private Rigidbody rb;
    private Vector3 offset;
    private float dragSpeed = 2000f;
    private bool controller = false;
    [Header(" Elements ")]
    [Tooltip(" Correct Position Object ")]
    [SerializeField]
    private GameObject correctPositionObject; // Serialized field for correct position GameObject
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
        if (correctPositionObject != null)
        {
            float threshold = 0.1f;

            if (controller == false)
            {
                // Calculate the distance in both x and y axes
                float xDistance = Mathf.Abs(transform.position.x - correctPositionObject.transform.position.x);
                float yDistance = Mathf.Abs(transform.position.y - correctPositionObject.transform.position.y);
                float zDistance = Mathf.Abs(transform.position.z - correctPositionObject.transform.position.z);

                // Check if both x and y positions are approximately equal to the correct positions
                if (xDistance < threshold && yDistance < threshold && zDistance < threshold)
                {
                    Debug.Log("Object is in the correct position!");
                    
                    // Trigger rotation animation if the object is a "PistonPart"
                    if (this.gameObject.tag == "PistonPart")
                    {
                        this.gameObject.GetComponent<Animator>().SetTrigger("Rotation");
                    }

                    correctPositionObject.SetActive(false);
                    this.gameObject.transform.position = correctPositionObject.transform.position;

                    controller = true;

                    // Count plus
                    GameManager.count++;
                }

                // Check if the distance is within the range (0.1 to 0.4) to activate/deactivate the correct position object
                if ((xDistance > 0.11 && xDistance < 0.5) || (yDistance > 0.11 && yDistance < 0.5) || (zDistance > 0.11 && zDistance < 0.5))
                {
                    correctPositionObject.SetActive(true);
                }
                else if (xDistance > 0.6f || yDistance > 0.6f || zDistance > 0.6f)
                {
                    correctPositionObject.SetActive(false);
                }
            }
            else
            {
                // Play idle animation and decrement count if the object is a "PistonPart"
                if (this.gameObject.tag == "PistonPart")
                {
                    this.gameObject.GetComponent<Animator>().Play("Idle");
                }

                GameManager.count--;
                controller = false;
                correctPositionObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Correct position object not assigned!");
        }
    }
    #endregion
}
