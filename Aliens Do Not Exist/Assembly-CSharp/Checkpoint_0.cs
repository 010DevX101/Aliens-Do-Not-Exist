using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class Checkpoint_0 : CheckpointLoader
{
	// Token: 0x0600002D RID: 45 RVA: 0x00003208 File Offset: 0x00001408
	public override void Load()
	{
		this._playerTransform.position = this._playerInitTransform.position;
		this._playerTransform.rotation = this._playerInitTransform.rotation;
		this._assassinTransform.position = this._assassinInitTransform.position;
		this._assassinTransform.rotation = this._assassinTransform.rotation;
	}

	// Token: 0x04000056 RID: 86
	[SerializeField]
	private Transform _playerTransform;

	// Token: 0x04000057 RID: 87
	[SerializeField]
	private Transform _playerInitTransform;

	// Token: 0x04000058 RID: 88
	[SerializeField]
	private Transform _assassinTransform;

	// Token: 0x04000059 RID: 89
	[SerializeField]
	private Transform _assassinInitTransform;
}
