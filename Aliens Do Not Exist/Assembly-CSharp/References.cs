using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class References : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000069 RID: 105 RVA: 0x000039BC File Offset: 0x00001BBC
	// (set) Token: 0x0600006A RID: 106 RVA: 0x000039C3 File Offset: 0x00001BC3
	public static References Instance { get; private set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600006B RID: 107 RVA: 0x000039CB File Offset: 0x00001BCB
	public GameObject PlayerGO
	{
		get
		{
			return this._playerGO;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600006C RID: 108 RVA: 0x000039D3 File Offset: 0x00001BD3
	public GameObject MutantGO
	{
		get
		{
			return this._mutantGO;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600006D RID: 109 RVA: 0x000039DB File Offset: 0x00001BDB
	public Transform VacioTransform
	{
		get
		{
			return this._vacioTransform;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600006E RID: 110 RVA: 0x000039E3 File Offset: 0x00001BE3
	// (set) Token: 0x0600006F RID: 111 RVA: 0x000039EB File Offset: 0x00001BEB
	public bool PlayerDeath { get; set; }

	// Token: 0x06000070 RID: 112 RVA: 0x000039F4 File Offset: 0x00001BF4
	private void Awake()
	{
		References.Instance = this;
	}

	// Token: 0x0400007A RID: 122
	[SerializeField]
	private GameObject _playerGO;

	// Token: 0x0400007B RID: 123
	[SerializeField]
	private GameObject _mutantGO;

	// Token: 0x0400007C RID: 124
	[SerializeField]
	private Transform _vacioTransform;
}
