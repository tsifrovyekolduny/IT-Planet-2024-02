using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float Sensitivity;
	public GameObject VideoPlane;
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

		
		Vector3 eulerRot = transform.rotation.eulerAngles;

		VideoPlane.transform.rotation = Quaternion.Euler(180, eulerRot.y + 180, 0);

		float yRot = (eulerRot.x + 180f) % 360f - 180f;	
		
		if (yRot > MaxRotationConstraint)
        {
			transform.rotation = Quaternion.Euler(MaxRotationConstraint, eulerRot.y, eulerRot.z);
		}
	}
}
