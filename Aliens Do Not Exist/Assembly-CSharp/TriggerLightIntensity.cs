using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class TriggerLightIntensity : MonoBehaviour
{
	// Token: 0x06000083 RID: 131 RVA: 0x00003B2E File Offset: 0x00001D2E
	private void Start()
	{
		this._spotLight = base.GetComponent<Light>();
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00003B3C File Offset: 0x00001D3C
	private void OnTriggerStay(Collider other)
	{
		this._spotLight.intensity = Mathf.Sin(this._time * this._velocity - this._velocity * 3.1415927f / this._range) / this._range + this._scale;
		this._time += Time.deltaTime;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00003B9A File Offset: 0x00001D9A
	private void OnTriggerExit(Collider other)
	{
		this._spotLight.intensity = this._velocity;
		this._time = 0f;
	}

	// Token: 0x04000087 RID: 135
	[SerializeField]
	private float _velocity = 1f;

	// Token: 0x04000088 RID: 136
	[SerializeField]
	private float _range = 2f;

	// Token: 0x04000089 RID: 137
	[SerializeField]
	private float _scale = 1.5f;

	// Token: 0x0400008A RID: 138
	private float _time;

	// Token: 0x0400008B RID: 139
	private Light _spotLight;
}
