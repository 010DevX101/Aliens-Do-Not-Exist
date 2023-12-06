using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class CheckpointsController : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000026 RID: 38 RVA: 0x000031D9 File Offset: 0x000013D9
	// (set) Token: 0x06000027 RID: 39 RVA: 0x000031E0 File Offset: 0x000013E0
	public static CheckpointsController Instance { get; private set; }

	// Token: 0x06000028 RID: 40 RVA: 0x000031E8 File Offset: 0x000013E8
	private void Awake()
	{
		CheckpointsController.Instance = this;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000031F0 File Offset: 0x000013F0
	private void Start()
	{
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000031F2 File Offset: 0x000013F2
	private void Update()
	{
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000031F4 File Offset: 0x000013F4
	public void SaveCheckpoint(CheckpointLoader checkpointLoader)
	{
		this._lastCheckpointLoader = checkpointLoader;
	}

	// Token: 0x04000055 RID: 85
	private CheckpointLoader _lastCheckpointLoader;
}
