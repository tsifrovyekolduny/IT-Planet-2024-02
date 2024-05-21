using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HubCameraMovement : MonoBehaviour
{
    public float Sensitivity;
    public GameObject VideoPlane;
    public float SpeedOfLookAt = 1f;
    public float StartTimeForBlockingCamera = 10f;
    public float Speed = 1f;
    public UnityEvent EventOnMovingToEnd = new UnityEvent();
    public UnityEvent EventOnUnblockingAfterTime = new UnityEvent();

    public Vector3 MaxRotation;
    public Vector3 MinRotaton;

    private bool _isBlocked = false;

    public void SetBlock(bool block)
    {
        _isBlocked = block;
    }

    IEnumerator UnblockCameraAfterTime()
    {
        yield return new WaitForSeconds(StartTimeForBlockingCamera);

        Debug.Log("Camera unblocked");
        _isBlocked = false;
        EventOnUnblockingAfterTime.Invoke();
    }

    private void Start()
    {
        if (StartTimeForBlockingCamera > 0)
        {
            Cursor.visible = false;
            _isBlocked = true;
            StartCoroutine(UnblockCameraAfterTime());
        }
    }

    public IEnumerator MoveCameraToPoint(Vector3 destination, bool withLookAt)
    {
        SetBlock(true);
        while (transform.position != destination)
        {
            if (withLookAt)
            {
                SmoothLookAt(destination);
            }
            
            transform.position = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            yield return null;
        }

        EventOnMovingToEnd.Invoke();
        yield break;
    }

    public void SmoothLookAt(Vector3 target)
    {
        Vector3 lTargetDir = target - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), SpeedOfLookAt);
    }

    void Update()
    {
        if (!_isBlocked)
        {
            float rotateHorizontal = Input.GetAxis("Mouse X");
            float rotateVertical = Input.GetAxis("Mouse Y");
            transform.RotateAround(transform.position, Vector3.up, rotateHorizontal * Sensitivity);
            transform.RotateAround(Vector3.zero, transform.right, rotateVertical * -Sensitivity);


            Vector3 eulerRot = transform.rotation.eulerAngles;

            if(VideoPlane != null)
            {
                VideoPlane.transform.rotation = Quaternion.Euler(180, eulerRot.y + 180, 0);
            }            

            eulerRot.y = (eulerRot.y + 180f) % 360f - 180f;
            eulerRot.x = (eulerRot.x + 180f) % 360f - 180f;
            eulerRot.z = (eulerRot.z + 180f) % 360f - 180f;
            eulerRot.y = Mathf.Clamp(eulerRot.y, MinRotaton.y, MaxRotation.y);
            eulerRot.z = Mathf.Clamp(eulerRot.z, MinRotaton.z, MaxRotation.z);
            eulerRot.x = Mathf.Clamp(eulerRot.x, MinRotaton.x, MaxRotation.x);

            transform.localRotation = Quaternion.Euler(eulerRot);
        }
    }
}
