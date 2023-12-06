using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class MutantCollider : MonoBehaviour
{
	// Token: 0x06000055 RID: 85 RVA: 0x00003638 File Offset: 0x00001838
	private void Start()
	{
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0000363A File Offset: 0x0000183A
	private void Update()
	{
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0000363C File Offset: 0x0000183C
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			MenusController.Instance.ActivateMenu();
			References.Instance.PlayerDeath = true;
			SoundsController.Instance.PlayOnDeath();
			SoundsController.Instance.StopStepsMutant();
			return;
		}
		if (other.CompareTag("Vacio"))
		{
			MutantLogic.Instance.Fall();
			SoundsController.Instance.PlayFallMutant();
			SoundsController.Instance.StopStepsMutant();
		}
	}
}
