using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class DisableRenderer : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x00002137 File Offset: 0x00000337
	private void Start()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002145 File Offset: 0x00000345
	private void Update()
	{
	}
}
