using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class EventsAnims : MonoBehaviour
{
	// Token: 0x06000036 RID: 54 RVA: 0x0000329B File Offset: 0x0000149B
	private void Start()
	{
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000329D File Offset: 0x0000149D
	private void Update()
	{
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000329F File Offset: 0x0000149F
	public void OnEndJumpAttackAnim()
	{
		Debug.Log("sissiis fin anim");
		MutantLogic.Instance.JumpAttackAnimFinished();
	}
}
