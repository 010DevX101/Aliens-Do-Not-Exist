using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class CharController_Motor : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x0000214F File Offset: 0x0000034F
	private void Start()
	{
		this.character = base.GetComponent<CharacterController>();
		if (Application.isEditor)
		{
			this.webGLRightClickRotation = false;
			this.sensitivity *= 1.5f;
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000217D File Offset: 0x0000037D
	private void CheckForWaterHeight()
	{
		if (base.transform.position.y < this.WaterHeight)
		{
			this.gravity = 0f;
			return;
		}
		this.gravity = -9.8f;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000021B0 File Offset: 0x000003B0
	private void Update()
	{
		this.moveFB = Input.GetAxis("Horizontal") * this.speed;
		this.moveLR = Input.GetAxis("Vertical") * this.speed;
		this.rotX = Input.GetAxis("Mouse X") * this.sensitivity;
		this.rotY = Input.GetAxis("Mouse Y") * this.sensitivity;
		this.CheckForWaterHeight();
		Vector3 vector;
		vector..ctor(this.moveFB, this.gravity, this.moveLR);
		if (this.webGLRightClickRotation)
		{
			if (Input.GetKey(323))
			{
				this.CameraRotation(this.cam, this.rotX, this.rotY);
			}
		}
		else if (!this.webGLRightClickRotation)
		{
			this.CameraRotation(this.cam, this.rotX, this.rotY);
		}
		vector = base.transform.rotation * vector;
		this.character.Move(vector * Time.deltaTime);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000022AF File Offset: 0x000004AF
	private void CameraRotation(GameObject cam, float rotX, float rotY)
	{
		base.transform.Rotate(0f, rotX * Time.deltaTime, 0f);
		cam.transform.Rotate(-rotY * Time.deltaTime, 0f, 0f);
	}

	// Token: 0x04000004 RID: 4
	public float speed = 10f;

	// Token: 0x04000005 RID: 5
	public float sensitivity = 30f;

	// Token: 0x04000006 RID: 6
	public float WaterHeight = 15.5f;

	// Token: 0x04000007 RID: 7
	private CharacterController character;

	// Token: 0x04000008 RID: 8
	public GameObject cam;

	// Token: 0x04000009 RID: 9
	private float moveFB;

	// Token: 0x0400000A RID: 10
	private float moveLR;

	// Token: 0x0400000B RID: 11
	private float rotX;

	// Token: 0x0400000C RID: 12
	private float rotY;

	// Token: 0x0400000D RID: 13
	public bool webGLRightClickRotation = true;

	// Token: 0x0400000E RID: 14
	private float gravity = -9.8f;
}
