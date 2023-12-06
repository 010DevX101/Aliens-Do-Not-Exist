using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class FirstPersonController : MonoBehaviour
{
	// Token: 0x06000012 RID: 18 RVA: 0x0000246C File Offset: 0x0000066C
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.crosshairObject = base.GetComponentInChildren<Image>();
		this.playerCamera.fieldOfView = this.fov;
		this.originalScale = base.transform.localScale;
		this.jointOriginalPos = this.joint.localPosition;
		if (!this.unlimitedSprint)
		{
			this.sprintRemaining = this.sprintDuration;
			this.sprintCooldownReset = this.sprintCooldown;
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000024E4 File Offset: 0x000006E4
	private void Start()
	{
		if (this.lockCursor)
		{
			Cursor.lockState = 1;
		}
		if (this.crosshair)
		{
			this.crosshairObject.sprite = this.crosshairImage;
			this.crosshairObject.color = this.crosshairColor;
		}
		else
		{
			this.crosshairObject.gameObject.SetActive(false);
		}
		this.sprintBarCG = base.GetComponentInChildren<CanvasGroup>();
		if (this.useSprintBar)
		{
			this.sprintBarBG.gameObject.SetActive(true);
			this.sprintBar.gameObject.SetActive(true);
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			this.sprintBarWidth = num * this.sprintBarWidthPercent;
			this.sprintBarHeight = num2 * this.sprintBarHeightPercent;
			this.sprintBarBG.rectTransform.sizeDelta = new Vector3(this.sprintBarWidth, this.sprintBarHeight, 0f);
			this.sprintBar.rectTransform.sizeDelta = new Vector3(this.sprintBarWidth - 2f, this.sprintBarHeight - 2f, 0f);
			if (this.hideBarWhenFull)
			{
				this.sprintBarCG.alpha = 0f;
				return;
			}
		}
		else
		{
			this.sprintBarBG.gameObject.SetActive(false);
			this.sprintBar.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000263C File Offset: 0x0000083C
	private void Update()
	{
		if (this.cameraCanMove)
		{
			this.yaw = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.mouseSensitivity;
			if (!this.invertCamera)
			{
				this.pitch -= this.mouseSensitivity * Input.GetAxis("Mouse Y");
			}
			else
			{
				this.pitch += this.mouseSensitivity * Input.GetAxis("Mouse Y");
			}
			this.pitch = Mathf.Clamp(this.pitch, -this.maxLookAngle, this.maxLookAngle);
			base.transform.localEulerAngles = new Vector3(0f, this.yaw, 0f);
			this.playerCamera.transform.localEulerAngles = new Vector3(this.pitch, 0f, 0f);
		}
		if (this.enableZoom)
		{
			if (Input.GetKeyDown(this.zoomKey) && !this.holdToZoom && !this.isSprinting)
			{
				if (!this.isZoomed)
				{
					this.isZoomed = true;
				}
				else
				{
					this.isZoomed = false;
				}
			}
			if (this.holdToZoom && !this.isSprinting)
			{
				if (Input.GetKeyDown(this.zoomKey))
				{
					this.isZoomed = true;
				}
				else if (Input.GetKeyUp(this.zoomKey))
				{
					this.isZoomed = false;
				}
			}
			if (this.isZoomed)
			{
				this.playerCamera.fieldOfView = Mathf.Lerp(this.playerCamera.fieldOfView, this.zoomFOV, this.zoomStepTime * Time.deltaTime);
			}
			else if (!this.isZoomed && !this.isSprinting)
			{
				this.playerCamera.fieldOfView = Mathf.Lerp(this.playerCamera.fieldOfView, this.fov, this.zoomStepTime * Time.deltaTime);
			}
		}
		if (this.enableSprint)
		{
			if (this.isSprinting)
			{
				this.isZoomed = false;
				this.playerCamera.fieldOfView = Mathf.Lerp(this.playerCamera.fieldOfView, this.sprintFOV, this.sprintFOVStepTime * Time.deltaTime);
				if (!this.unlimitedSprint)
				{
					this.sprintRemaining -= 1f * Time.deltaTime;
					if (this.sprintRemaining <= 0f)
					{
						this.isSprinting = false;
						this.isSprintCooldown = true;
					}
				}
			}
			else
			{
				this.sprintRemaining = Mathf.Clamp(this.sprintRemaining += 1f * Time.deltaTime, 0f, this.sprintDuration);
			}
			if (this.isSprintCooldown)
			{
				this.sprintCooldown -= 1f * Time.deltaTime;
				if (this.sprintCooldown <= 0f)
				{
					this.isSprintCooldown = false;
				}
			}
			else
			{
				this.sprintCooldown = this.sprintCooldownReset;
			}
			if (this.useSprintBar && !this.unlimitedSprint)
			{
				float num = this.sprintRemaining / this.sprintDuration;
				this.sprintBar.transform.localScale = new Vector3(num, 1f, 1f);
			}
		}
		if (this.enableJump && Input.GetKeyDown(this.jumpKey) && this.isGrounded)
		{
			this.Jump();
		}
		if (this.enableCrouch)
		{
			if (Input.GetKeyDown(this.crouchKey) && !this.holdToCrouch)
			{
				this.Crouch();
			}
			if (Input.GetKeyDown(this.crouchKey) && this.holdToCrouch)
			{
				this.isCrouched = false;
				this.Crouch();
			}
			else if (Input.GetKeyUp(this.crouchKey) && this.holdToCrouch)
			{
				this.isCrouched = true;
				this.Crouch();
			}
		}
		this.CheckGround();
		if (this.enableHeadBob)
		{
			this.HeadBob();
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000029E8 File Offset: 0x00000BE8
	private void FixedUpdate()
	{
		if (this.playerCanMove)
		{
			Vector3 vector;
			vector..ctor(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			if (vector.x != 0f || (vector.z != 0f && this.isGrounded))
			{
				this.isWalking = true;
			}
			else
			{
				this.isWalking = false;
			}
			if (this.enableSprint && Input.GetKey(this.sprintKey) && this.sprintRemaining > 0f && !this.isSprintCooldown)
			{
				vector = base.transform.TransformDirection(vector) * this.sprintSpeed;
				Vector3 velocity = this.rb.velocity;
				Vector3 vector2 = vector - velocity;
				vector2.x = Mathf.Clamp(vector2.x, -this.maxVelocityChange, this.maxVelocityChange);
				vector2.z = Mathf.Clamp(vector2.z, -this.maxVelocityChange, this.maxVelocityChange);
				vector2.y = 0f;
				if (vector2.x != 0f || vector2.z != 0f)
				{
					this.isSprinting = true;
					if (this.isCrouched)
					{
						this.Crouch();
					}
					if (this.hideBarWhenFull && !this.unlimitedSprint)
					{
						this.sprintBarCG.alpha += 5f * Time.deltaTime;
					}
				}
				this.rb.AddForce(vector2, 2);
				return;
			}
			this.isSprinting = false;
			if (this.hideBarWhenFull && this.sprintRemaining == this.sprintDuration)
			{
				this.sprintBarCG.alpha -= 3f * Time.deltaTime;
			}
			vector = base.transform.TransformDirection(vector) * this.walkSpeed;
			Vector3 velocity2 = this.rb.velocity;
			Vector3 vector3 = vector - velocity2;
			vector3.x = Mathf.Clamp(vector3.x, -this.maxVelocityChange, this.maxVelocityChange);
			vector3.z = Mathf.Clamp(vector3.z, -this.maxVelocityChange, this.maxVelocityChange);
			vector3.y = 0f;
			this.rb.AddForce(vector3, 2);
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002C2C File Offset: 0x00000E2C
	private void CheckGround()
	{
		Vector3 vector;
		vector..ctor(base.transform.position.x, base.transform.position.y - base.transform.localScale.y * 0.5f, base.transform.position.z);
		Vector3 vector2 = base.transform.TransformDirection(Vector3.down);
		float num = 0.75f;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, vector2, ref raycastHit, num))
		{
			Debug.DrawRay(vector, vector2 * num, Color.red);
			this.isGrounded = true;
			return;
		}
		this.isGrounded = false;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002CCC File Offset: 0x00000ECC
	private void Jump()
	{
		if (this.isGrounded)
		{
			this.rb.AddForce(0f, this.jumpPower, 0f, 1);
			this.isGrounded = false;
		}
		if (this.isCrouched && !this.holdToCrouch)
		{
			this.Crouch();
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002D1C File Offset: 0x00000F1C
	private void Crouch()
	{
		if (this.isCrouched)
		{
			base.transform.localScale = new Vector3(this.originalScale.x, this.originalScale.y, this.originalScale.z);
			this.walkSpeed /= this.speedReduction;
			this.isCrouched = false;
			return;
		}
		base.transform.localScale = new Vector3(this.originalScale.x, this.crouchHeight, this.originalScale.z);
		this.walkSpeed *= this.speedReduction;
		this.isCrouched = true;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002DC4 File Offset: 0x00000FC4
	private void HeadBob()
	{
		if (this.isWalking)
		{
			if (this.isSprinting)
			{
				this.timer += Time.deltaTime * (this.bobSpeed + this.sprintSpeed);
			}
			else if (this.isCrouched)
			{
				this.timer += Time.deltaTime * (this.bobSpeed * this.speedReduction);
			}
			else
			{
				this.timer += Time.deltaTime * this.bobSpeed;
			}
			this.joint.localPosition = new Vector3(this.jointOriginalPos.x + Mathf.Sin(this.timer) * this.bobAmount.x, this.jointOriginalPos.y + Mathf.Sin(this.timer) * this.bobAmount.y, this.jointOriginalPos.z + Mathf.Sin(this.timer) * this.bobAmount.z);
			return;
		}
		this.timer = 0f;
		this.joint.localPosition = new Vector3(Mathf.Lerp(this.joint.localPosition.x, this.jointOriginalPos.x, Time.deltaTime * this.bobSpeed), Mathf.Lerp(this.joint.localPosition.y, this.jointOriginalPos.y, Time.deltaTime * this.bobSpeed), Mathf.Lerp(this.joint.localPosition.z, this.jointOriginalPos.z, Time.deltaTime * this.bobSpeed));
	}

	// Token: 0x04000011 RID: 17
	private Rigidbody rb;

	// Token: 0x04000012 RID: 18
	public Camera playerCamera;

	// Token: 0x04000013 RID: 19
	public float fov = 60f;

	// Token: 0x04000014 RID: 20
	public bool invertCamera;

	// Token: 0x04000015 RID: 21
	public bool cameraCanMove = true;

	// Token: 0x04000016 RID: 22
	public float mouseSensitivity = 2f;

	// Token: 0x04000017 RID: 23
	public float maxLookAngle = 50f;

	// Token: 0x04000018 RID: 24
	public bool lockCursor = true;

	// Token: 0x04000019 RID: 25
	public bool crosshair = true;

	// Token: 0x0400001A RID: 26
	public Sprite crosshairImage;

	// Token: 0x0400001B RID: 27
	public Color crosshairColor = Color.white;

	// Token: 0x0400001C RID: 28
	private float yaw;

	// Token: 0x0400001D RID: 29
	private float pitch;

	// Token: 0x0400001E RID: 30
	private Image crosshairObject;

	// Token: 0x0400001F RID: 31
	public bool enableZoom = true;

	// Token: 0x04000020 RID: 32
	public bool holdToZoom;

	// Token: 0x04000021 RID: 33
	public KeyCode zoomKey = 324;

	// Token: 0x04000022 RID: 34
	public float zoomFOV = 30f;

	// Token: 0x04000023 RID: 35
	public float zoomStepTime = 5f;

	// Token: 0x04000024 RID: 36
	private bool isZoomed;

	// Token: 0x04000025 RID: 37
	public bool playerCanMove = true;

	// Token: 0x04000026 RID: 38
	public float walkSpeed = 5f;

	// Token: 0x04000027 RID: 39
	public float maxVelocityChange = 10f;

	// Token: 0x04000028 RID: 40
	private bool isWalking;

	// Token: 0x04000029 RID: 41
	public bool enableSprint = true;

	// Token: 0x0400002A RID: 42
	public bool unlimitedSprint;

	// Token: 0x0400002B RID: 43
	public KeyCode sprintKey = 304;

	// Token: 0x0400002C RID: 44
	public float sprintSpeed = 7f;

	// Token: 0x0400002D RID: 45
	public float sprintDuration = 5f;

	// Token: 0x0400002E RID: 46
	public float sprintCooldown = 0.5f;

	// Token: 0x0400002F RID: 47
	public float sprintFOV = 80f;

	// Token: 0x04000030 RID: 48
	public float sprintFOVStepTime = 10f;

	// Token: 0x04000031 RID: 49
	public bool useSprintBar = true;

	// Token: 0x04000032 RID: 50
	public bool hideBarWhenFull = true;

	// Token: 0x04000033 RID: 51
	public Image sprintBarBG;

	// Token: 0x04000034 RID: 52
	public Image sprintBar;

	// Token: 0x04000035 RID: 53
	public float sprintBarWidthPercent = 0.3f;

	// Token: 0x04000036 RID: 54
	public float sprintBarHeightPercent = 0.015f;

	// Token: 0x04000037 RID: 55
	private CanvasGroup sprintBarCG;

	// Token: 0x04000038 RID: 56
	private bool isSprinting;

	// Token: 0x04000039 RID: 57
	private float sprintRemaining;

	// Token: 0x0400003A RID: 58
	private float sprintBarWidth;

	// Token: 0x0400003B RID: 59
	private float sprintBarHeight;

	// Token: 0x0400003C RID: 60
	private bool isSprintCooldown;

	// Token: 0x0400003D RID: 61
	private float sprintCooldownReset;

	// Token: 0x0400003E RID: 62
	public bool enableJump = true;

	// Token: 0x0400003F RID: 63
	public KeyCode jumpKey = 32;

	// Token: 0x04000040 RID: 64
	public float jumpPower = 5f;

	// Token: 0x04000041 RID: 65
	private bool isGrounded;

	// Token: 0x04000042 RID: 66
	public bool enableCrouch = true;

	// Token: 0x04000043 RID: 67
	public bool holdToCrouch = true;

	// Token: 0x04000044 RID: 68
	public KeyCode crouchKey = 306;

	// Token: 0x04000045 RID: 69
	public float crouchHeight = 0.75f;

	// Token: 0x04000046 RID: 70
	public float speedReduction = 0.5f;

	// Token: 0x04000047 RID: 71
	private bool isCrouched;

	// Token: 0x04000048 RID: 72
	private Vector3 originalScale;

	// Token: 0x04000049 RID: 73
	public bool enableHeadBob = true;

	// Token: 0x0400004A RID: 74
	public Transform joint;

	// Token: 0x0400004B RID: 75
	public float bobSpeed = 10f;

	// Token: 0x0400004C RID: 76
	public Vector3 bobAmount = new Vector3(0.15f, 0.05f, 0f);

	// Token: 0x0400004D RID: 77
	private Vector3 jointOriginalPos;

	// Token: 0x0400004E RID: 78
	private float timer;

	// Token: 0x0400004F RID: 79
	private float camRotation;
}
