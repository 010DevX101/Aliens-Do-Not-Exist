using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class NotesCounter : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000063 RID: 99 RVA: 0x000038FD File Offset: 0x00001AFD
	// (set) Token: 0x06000064 RID: 100 RVA: 0x00003904 File Offset: 0x00001B04
	public static NotesCounter Instance { get; private set; }

	// Token: 0x06000065 RID: 101 RVA: 0x0000390C File Offset: 0x00001B0C
	private void Awake()
	{
		NotesCounter.Instance = this;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003914 File Offset: 0x00001B14
	private void Start()
	{
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003918 File Offset: 0x00001B18
	public void AddNoteCount(int note)
	{
		if (!this._notesFound.Contains(note))
		{
			this._notesFound.Add(note);
			this._textMeshProUGUI.text = "Notas encontradas: " + this._notesFound.Count.ToString() + "/10";
			if (this._notesFound.Count >= 10)
			{
				References.Instance.MutantGO.SetActive(true);
				SoundsController.Instance.PlayRoarMutant();
				SoundsController.Instance.PlayStepsMutant();
				SoundsController.Instance.PlayAmbientMusic();
			}
		}
	}

	// Token: 0x04000076 RID: 118
	[SerializeField]
	private TextMeshProUGUI _textMeshProUGUI;

	// Token: 0x04000077 RID: 119
	private List<int> _notesFound = new List<int>();
}
