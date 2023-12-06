using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class FootstepsPlayerSoundActivator : MonoBehaviour
{
	// Token: 0x06000072 RID: 114 RVA: 0x00003A04 File Offset: 0x00001C04
	private void FixedUpdate()
	{
		if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
		{
			if (this._onPause)
			{
				SoundsController.Instance.ContinueFootsteps();
				this._onPause = false;
				return;
			}
		}
		else if (!this._onPause)
		{
			SoundsController.Instance.PauseFootsteps();
			this._onPause = true;
		}
	}

	// Token: 0x0400007D RID: 125
	private bool _onPause = true;
}
