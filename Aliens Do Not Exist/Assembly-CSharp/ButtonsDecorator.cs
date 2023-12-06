using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000008 RID: 8
public class ButtonsDecorator : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600001B RID: 27 RVA: 0x000030DF File Offset: 0x000012DF
	private void Start()
	{
		this._textMeshProUGUI = base.GetComponent<TextMeshProUGUI>();
		this._originalColor = this._textMeshProUGUI.color;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000030FE File Offset: 0x000012FE
	public void OnPointerEnter(PointerEventData eventData)
	{
		base.transform.localScale *= this._scale;
		this._textMeshProUGUI.color = this._color;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000312D File Offset: 0x0000132D
	public void OnPointerExit(PointerEventData eventData)
	{
		base.transform.localScale /= this._scale;
		this._textMeshProUGUI.color = this._originalColor;
	}

	// Token: 0x04000050 RID: 80
	[SerializeField]
	private float _scale = 1.2f;

	// Token: 0x04000051 RID: 81
	[SerializeField]
	private Color _color = Color.red;

	// Token: 0x04000052 RID: 82
	private TextMeshProUGUI _textMeshProUGUI;

	// Token: 0x04000053 RID: 83
	private Color _originalColor;
}
