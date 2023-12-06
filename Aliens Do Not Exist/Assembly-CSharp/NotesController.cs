using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class NotesController : MonoBehaviour
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600003C RID: 60 RVA: 0x000032D7 File Offset: 0x000014D7
	// (set) Token: 0x0600003D RID: 61 RVA: 0x000032DE File Offset: 0x000014DE
	public static NotesController Instance { get; private set; }

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600003E RID: 62 RVA: 0x000032E6 File Offset: 0x000014E6
	// (set) Token: 0x0600003F RID: 63 RVA: 0x000032EE File Offset: 0x000014EE
	public bool NoteIsOnScreen { get; private set; }

	// Token: 0x06000040 RID: 64 RVA: 0x000032F7 File Offset: 0x000014F7
	private void Awake()
	{
		NotesController.Instance = this;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000032FF File Offset: 0x000014FF
	private void Start()
	{
		this._firstPersonController = References.Instance.PlayerGO.GetComponent<FirstPersonController>();
		this._interactionsPlayer = References.Instance.PlayerGO.GetComponent<InteractionsPlayer>();
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000332C File Offset: 0x0000152C
	public void ShowNote(int noteID)
	{
		this._lastNoteID = noteID;
		this._lastNoteGO = base.transform.GetChild(noteID).gameObject;
		this._lastNoteGO.SetActive(true);
		this._firstPersonController.playerCanMove = false;
		this._firstPersonController.cameraCanMove = false;
		SoundsController.Instance.PlayNotes();
		this._interactionsPlayer.canInteract = false;
		this.NoteIsOnScreen = true;
		MonoBehaviour.print("ShowNote");
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000033A4 File Offset: 0x000015A4
	public void OcultLastNote()
	{
		this._lastNoteGO.SetActive(false);
		this._firstPersonController.playerCanMove = true;
		this._firstPersonController.cameraCanMove = true;
		this._interactionsPlayer.canInteract = true;
		NotesCounter.Instance.AddNoteCount(this._lastNoteID);
		this.NoteIsOnScreen = false;
	}

	// Token: 0x0400005D RID: 93
	private GameObject _lastNoteGO;

	// Token: 0x0400005E RID: 94
	private int _lastNoteID;

	// Token: 0x0400005F RID: 95
	private FirstPersonController _firstPersonController;

	// Token: 0x04000060 RID: 96
	private InteractionsPlayer _interactionsPlayer;
}
