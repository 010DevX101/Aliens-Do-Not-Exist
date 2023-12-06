using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class CursorLocker : MonoBehaviour
{
	// Token: 0x0600002F RID: 47 RVA: 0x00003275 File Offset: 0x00001475
	private void Awake()
	{
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003277 File Offset: 0x00001477
	private void Start()
	{
		Cursor.lockState = 1;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000327F File Offset: 0x0000147F
	private void Update()
	{
	}
}
