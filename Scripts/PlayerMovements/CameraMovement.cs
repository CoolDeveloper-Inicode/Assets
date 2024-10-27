using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity;

    [Header("Camera Collision Settings")]
    public float camDistance;
    public Vector2 cameraDistanceMinMax = new Vector2(0.5f, 5f);
    public Transform cam;

    public Transform orientation;

    public LayerMask layersToIgnore;

    [Header("Lock On Settings")]
    public Transform lockOnTargetTransform;
    public Transform lockOnPosition;
    public Transform currentCameraPosition;

    [HideInInspector]
    public bool lockOnFlag;

    Vector3 cameraDirection;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraDirection = cam.transform.localPosition.normalized;
        camDistance = cameraDistanceMinMax.y;

        currentCameraPosition.position = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && lockOnTargetTransform != null)
        {
            if (lockOnFlag)
            {
                lockOnFlag = false;
            }
            else
            {
                lockOnFlag = true;
            }
        }

        if (lockOnFlag)
        {
            Vector3 targetPosition = lockOnTargetTransform.position - transform.position;
            targetPosition.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 35f * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, lockOnPosition.position, 20f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(lockOnPosition.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            orientation.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            transform.position = currentCameraPosition.position;
        }

        CheckForCameraCollisions();
    }

    void CheckForCameraCollisions()
    {
        Vector3 desiredCameraPosition = transform.TransformPoint(cameraDirection * cameraDistanceMinMax.y);
        RaycastHit hit;

        if (Physics.Linecast (transform.position, desiredCameraPosition, out hit, layersToIgnore))
        {
            camDistance = Mathf.Clamp(hit.distance, cameraDistanceMinMax.x, cameraDistanceMinMax.y);
        }
        else
        {
            camDistance = cameraDistanceMinMax.y;
        }

        cam.transform.localPosition = cameraDirection * camDistance;
    }
}
