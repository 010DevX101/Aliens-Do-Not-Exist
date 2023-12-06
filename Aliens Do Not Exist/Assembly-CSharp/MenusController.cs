using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000014 RID: 20
public class MenusController : MonoBehaviour
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600004B RID: 75 RVA: 0x000034D6 File Offset: 0x000016D6
	// (set) Token: 0x0600004C RID: 76 RVA: 0x000034DD File Offset: 0x000016DD
	public static MenusController Instance { get; private set; }

	// Token: 0x0600004D RID: 77 RVA: 0x000034E5 File Offset: 0x000016E5
	private void Awake()
	{
		MenusController.Instance = this;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000034ED File Offset: 0x000016ED
	private void Start()
	{
		this._firstPersonController = References.Instance.PlayerGO.GetComponent<FirstPersonController>();
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003504 File Offset: 0x00001704
	private void Update()
	{
		if (!References.Instance.PlayerDeath && Input.GetKeyDown(112))
		{
			if (this._pauseGO.activeSelf)
			{
				this.Continuar();
				return;
			}
			this._pauseGO.SetActive(true);
			SoundsController.Instance.PauseFootsteps();
			this._firstPersonController.playerCanMove = false;
			this._firstPersonController.cameraCanMove = false;
			Cursor.lockState = 0;
			Time.timeScale = 0f;
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003578 File Offset: 0x00001778
	public void ActivateMenu()
	{
		this._menuGO.SetActive(true);
		SoundsController.Instance.PauseFootsteps();
		this._firstPersonController.playerCanMove = false;
		this._firstPersonController.cameraCanMove = false;
		Cursor.lockState = 0;
		Time.timeScale = 0f;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000035B8 File Offset: 0x000017B8
	public void Continuar()
	{
		this._pauseGO.SetActive(false);
		SoundsController.Instance.ContinueFootsteps();
		this._firstPersonController.playerCanMove = true;
		this._firstPersonController.cameraCanMove = true;
		Cursor.lockState = 1;
		Time.timeScale = 1f;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000035F8 File Offset: 0x000017F8
	public void Reiniciar()
	{
		Time.timeScale = 1f;
		Cursor.lockState = 1;
		SceneManager.LoadScene("Game");
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003614 File Offset: 0x00001814
	public void Salir()
	{
		Time.timeScale = 1f;
		Cursor.lockState = 0;
		SceneManager.LoadScene("Menu");
	}

	// Token: 0x04000067 RID: 103
	[SerializeField]
	private GameObject _pauseGO;

	// Token: 0x04000068 RID: 104
	[SerializeField]
	private GameObject _menuGO;

	// Token: 0x04000069 RID: 105
	private FirstPersonController _firstPersonController;
}
