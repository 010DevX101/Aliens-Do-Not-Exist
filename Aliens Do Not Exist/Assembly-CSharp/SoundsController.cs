using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class SoundsController : MonoBehaviour
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000074 RID: 116 RVA: 0x00003A75 File Offset: 0x00001C75
	// (set) Token: 0x06000075 RID: 117 RVA: 0x00003A7C File Offset: 0x00001C7C
	public static SoundsController Instance { get; private set; }

	// Token: 0x06000076 RID: 118 RVA: 0x00003A84 File Offset: 0x00001C84
	private void Awake()
	{
		SoundsController.Instance = this;
		this._footstepsAudio.Play();
		this._footstepsAudio.Pause();
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00003AA2 File Offset: 0x00001CA2
	private void Update()
	{
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00003AA4 File Offset: 0x00001CA4
	public void ContinueFootsteps()
	{
		this._footstepsAudio.UnPause();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00003AB1 File Offset: 0x00001CB1
	public void PauseFootsteps()
	{
		this._footstepsAudio.Pause();
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00003ABE File Offset: 0x00001CBE
	public void PlayNotes()
	{
		this._notesAudio.Play();
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00003ACB File Offset: 0x00001CCB
	public void PlayRoarMutant()
	{
		this._roarMutant.Play();
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00003AD8 File Offset: 0x00001CD8
	public void PlayOnDeath()
	{
		this._onDeath.Play();
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00003AE5 File Offset: 0x00001CE5
	public void PlayStepsMutant()
	{
		this._stepsMutant.Play();
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00003AF2 File Offset: 0x00001CF2
	public void StopStepsMutant()
	{
		this._stepsMutant.Stop();
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00003AFF File Offset: 0x00001CFF
	public void PlayFallMutant()
	{
		this._fallMutant.Play();
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00003B0C File Offset: 0x00001D0C
	public void PlayExplosion()
	{
		this._explosion.Play();
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00003B19 File Offset: 0x00001D19
	public void PlayAmbientMusic()
	{
		this._ambient.Play();
	}

	// Token: 0x0400007F RID: 127
	[SerializeField]
	private AudioSource _footstepsAudio;

	// Token: 0x04000080 RID: 128
	[SerializeField]
	private AudioSource _notesAudio;

	// Token: 0x04000081 RID: 129
	[SerializeField]
	private AudioSource _ambient;

	// Token: 0x04000082 RID: 130
	[SerializeField]
	private AudioSource _roarMutant;

	// Token: 0x04000083 RID: 131
	[SerializeField]
	private AudioSource _onDeath;

	// Token: 0x04000084 RID: 132
	[SerializeField]
	private AudioSource _stepsMutant;

	// Token: 0x04000085 RID: 133
	[SerializeField]
	private AudioSource _fallMutant;

	// Token: 0x04000086 RID: 134
	[SerializeField]
	private AudioSource _explosion;
}
