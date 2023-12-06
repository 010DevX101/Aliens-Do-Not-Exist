using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class NoteInteractable : MonoBehaviour, IInteractable
{
	// Token: 0x0600003A RID: 58 RVA: 0x000032BD File Offset: 0x000014BD
	public void Interact()
	{
		NotesController.Instance.ShowNote(this._noteID);
	}

	// Token: 0x0400005A RID: 90
	[SerializeField]
	private int _noteID;
}
