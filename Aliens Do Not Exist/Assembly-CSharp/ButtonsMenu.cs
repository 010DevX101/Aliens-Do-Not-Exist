using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000009 RID: 9
public class ButtonsMenu : MonoBehaviour
{
	// Token: 0x0600001F RID: 31 RVA: 0x0000317A File Offset: 0x0000137A
	public void Jugar()
	{
		SceneManager.LoadScene("Game");
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003186 File Offset: 0x00001386
	public void Salir()
	{
		Application.Quit();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000318D File Offset: 0x0000138D
	public void ComprarUnaHamburguesa()
	{
		Application.OpenURL("https://listado.mercadolibre.com.mx/hamburguesa#D[A:hamburguesa]");
	}
}
