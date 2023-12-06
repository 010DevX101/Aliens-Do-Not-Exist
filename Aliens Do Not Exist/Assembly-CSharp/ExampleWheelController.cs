using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
	private void Update()
	{
		if (Input.GetKey(273))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), 5);
		}
		else if (Input.GetKey(274))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), 5);
		}
		float num = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(num, -0.25f, 0.25f));
		}
	}

	// Token: 0x04000001 RID: 1
	public float acceleration;

	// Token: 0x04000002 RID: 2
	public Renderer motionVectorRenderer;

	// Token: 0x04000003 RID: 3
	private Rigidbody m_Rigidbody;

	// Token: 0x0200004D RID: 77
	private static class Uniforms
	{
		// Token: 0x04000113 RID: 275
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
