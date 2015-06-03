using UnityEngine;
using System.Collections;

public class BougeTest : MonoBehaviour 
{
	 float minFov = 5f;
	 float maxFov = 130f;
	 float sensitivity = 25;
	 float xAxisValue;
	 float yAxisValue;
	 float zAxisValue;

	 enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	 RotationAxes axes = RotationAxes.MouseXAndY;
	 float sensitivityX = 15F;
	 float sensitivityY = 15F;
	 float rotationY = 0F;
	
	 float minimumX = -360F;
	 float maximumX = 360F;
	
	 float minimumY = -60F;
	 float maximumY = 60F;
	
		
		void Start()
		{
			//camera.orthographic = true;
			//Debug.Log (Camera.main.isOrthoGraphic);
		}
		
		void Update () 
		{
				float fov = camera.fieldOfView;
				fov -= Input.GetAxis ("Mouse ScrollWheel") * sensitivity;
				fov = Mathf.Clamp (fov, minFov, maxFov);
				camera.fieldOfView = fov;
		}
		
		void FixedUpdate()
		{
			xAxisValue = Input.GetAxis("Horizontal");
			zAxisValue = Input.GetAxis("Vertical");

			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
			
			if(this.GetComponent<Camera>().enabled)
			{
				camera.transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
			}
		}
}
