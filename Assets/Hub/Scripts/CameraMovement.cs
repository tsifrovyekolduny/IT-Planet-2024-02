using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float Sensitivity;
	public float MinRotationConstraint = -90f;
	public float MaxRotationConstraint = 0f;

	private void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
	}

    void FixedUpdate()
	{
		float rotateHorizontal = Input.GetAxis("Mouse X");
		float rotateVertical = Input.GetAxis("Mouse Y");
		transform.RotateAround(transform.position, Vector3.up, rotateHorizontal * Sensitivity);
		transform.RotateAround(Vector3.zero, transform.right, rotateVertical * -Sensitivity);
	}
}
