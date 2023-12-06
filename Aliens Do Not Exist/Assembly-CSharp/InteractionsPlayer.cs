using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class InteractionsPlayer : MonoBehaviour
{
	// Token: 0x06000046 RID: 70 RVA: 0x00003400 File Offset: 0x00001600
	private void Start()
	{
		this._mainCameraTransform = Camera.main.transform;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003412 File Offset: 0x00001612
	private void Update()
	{
		this.DetectInteractions();
		this.ManageNoteOnScreen();
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003420 File Offset: 0x00001620
	private void DetectInteractions()
	{
		RaycastHit raycastHit;
		if (this.canInteract && Input.GetMouseButtonDown(0) && Physics.Raycast(this._mainCameraTransform.position, this._mainCameraTransform.forward, ref raycastHit, this._distance, this._layerMask))
		{
			raycastHit.transform.GetComponent<IInteractable>().Interact();
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x0000347E File Offset: 0x0000167E
	private void ManageNoteOnScreen()
	{
		if (NotesController.Instance.NoteIsOnScreen)
		{
			if (Input.GetMouseButtonUp(0))
			{
				this._mouseUpOnAPastFrame = true;
			}
			if (this._mouseUpOnAPastFrame && Input.GetMouseButtonDown(0))
			{
				NotesController.Instance.OcultLastNote();
				this._mouseUpOnAPastFrame = false;
			}
		}
	}

	// Token: 0x04000061 RID: 97
	public bool canInteract = true;

	// Token: 0x04000062 RID: 98
	[SerializeField]
	private float _distance = 3f;

	// Token: 0x04000063 RID: 99
	[SerializeField]
	private LayerMask _layerMask;

	// Token: 0x04000064 RID: 100
	private Transform _mainCameraTransform;

	// Token: 0x04000065 RID: 101
	private bool _mouseUpOnAPastFrame;
}
