using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class VolverAlMenu : MonoBehaviour
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000087 RID: 135 RVA: 0x00003BE1 File Offset: 0x00001DE1
	// (set) Token: 0x06000088 RID: 136 RVA: 0x00003BE8 File Offset: 0x00001DE8
	public static VolverAlMenu Instance { get; private set; }

	// Token: 0x06000089 RID: 137 RVA: 0x00003BF0 File Offset: 0x00001DF0
	private void Awake()
	{
		VolverAlMenu.Instance = this;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00003BF8 File Offset: 0x00001DF8
	private void Update()
	{
		if (this._final && Input.anyKey)
		{
			Application.Quit();
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00003C0E File Offset: 0x00001E0E
	private void Start()
	{
		this._anim = base.GetComponent<Animation>();
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00003C1C File Offset: 0x00001E1C
	public void PlayAnim()
	{
		this._anim.Play();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00003C2A File Offset: 0x00001E2A
	public void Final()
	{
		this._final = true;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00003C33 File Offset: 0x00001E33
	public void FinalFinal()
	{
		Time.timeScale = 0f;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00003C3F File Offset: 0x00001E3F
	public void NoMov()
	{
		FirstPersonController component = References.Instance.PlayerGO.GetComponent<FirstPersonController>();
		component.cameraCanMove = false;
		component.playerCanMove = false;
	}

	// Token: 0x0400008D RID: 141
	private Animation _anim;

	// Token: 0x0400008E RID: 142
	private bool _final;
}
