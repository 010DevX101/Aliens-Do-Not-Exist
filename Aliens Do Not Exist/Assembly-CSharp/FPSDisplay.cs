using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class FPSDisplay : MonoBehaviour
{
	// Token: 0x0600000C RID: 12 RVA: 0x00002325 File Offset: 0x00000525
	private void Update()
	{
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002348 File Offset: 0x00000548
	private void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;
		GUIStyle guistyle = new GUIStyle();
		Rect rect = new Rect(0f, 0f, (float)width, (float)(height * 2 / 100));
		guistyle.alignment = 0;
		guistyle.fontSize = height * 2 / 100;
		guistyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
		float num = this.deltaTime * 1000f;
		float num2 = 1f / this.deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
		GUI.Label(rect, text, guistyle);
	}

	// Token: 0x0400000F RID: 15
	private float deltaTime;
}
