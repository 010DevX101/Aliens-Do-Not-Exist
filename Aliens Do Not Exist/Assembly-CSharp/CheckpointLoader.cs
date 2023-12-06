using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public abstract class CheckpointLoader : MonoBehaviour
{
	// Token: 0x06000023 RID: 35 RVA: 0x000031A1 File Offset: 0x000013A1
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == References.Instance.PlayerGO)
		{
			CheckpointsController.Instance.SaveCheckpoint(this);
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000024 RID: 36
	public abstract void Load();
}
