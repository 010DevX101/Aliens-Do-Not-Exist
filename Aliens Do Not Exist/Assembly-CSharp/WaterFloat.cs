using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class WaterFloat : MonoBehaviour
{
	// Token: 0x0600000F RID: 15 RVA: 0x000023FA File Offset: 0x000005FA
	private void Start()
	{
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000023FC File Offset: 0x000005FC
	private void Update()
	{
		if (base.transform.position.y < this.WaterHeight)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.WaterHeight, base.transform.position.z);
		}
	}

	// Token: 0x04000010 RID: 16
	public float WaterHeight = 15.5f;
}
