using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000049 RID: 73
	public static class GraphicsUtils
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008E3C File Offset: 0x0000703C
		public static bool isLinearColorSpace
		{
			get
			{
				return QualitySettings.activeColorSpace == 1;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00008E46 File Offset: 0x00007046
		public static bool supportsDX11
		{
			get
			{
				return SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00008E58 File Offset: 0x00007058
		public static Texture2D whiteTexture
		{
			get
			{
				if (GraphicsUtils.s_WhiteTexture != null)
				{
					return GraphicsUtils.s_WhiteTexture;
				}
				GraphicsUtils.s_WhiteTexture = new Texture2D(1, 1, 5, false);
				GraphicsUtils.s_WhiteTexture.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
				GraphicsUtils.s_WhiteTexture.Apply();
				return GraphicsUtils.s_WhiteTexture;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00008EBC File Offset: 0x000070BC
		public static Mesh quad
		{
			get
			{
				if (GraphicsUtils.s_Quad != null)
				{
					return GraphicsUtils.s_Quad;
				}
				Vector3[] vertices = new Vector3[]
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f)
				};
				Vector2[] uv = new Vector2[]
				{
					new Vector2(0f, 0f),
					new Vector2(1f, 1f),
					new Vector2(1f, 0f),
					new Vector2(0f, 1f)
				};
				int[] triangles = new int[]
				{
					0,
					1,
					2,
					1,
					0,
					3
				};
				GraphicsUtils.s_Quad = new Mesh
				{
					vertices = vertices,
					uv = uv,
					triangles = triangles
				};
				GraphicsUtils.s_Quad.RecalculateNormals();
				GraphicsUtils.s_Quad.RecalculateBounds();
				return GraphicsUtils.s_Quad;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008FF8 File Offset: 0x000071F8
		public static void Blit(Material material, int pass)
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			material.SetPass(pass);
			GL.Begin(5);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000090B4 File Offset: 0x000072B4
		public static void ClearAndBlit(Texture source, RenderTexture destination, Material material, int pass, bool clearColor = true, bool clearDepth = false)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = destination;
			GL.Clear(false, clearColor, Color.clear);
			GL.PushMatrix();
			GL.LoadOrtho();
			material.SetTexture("_MainTex", source);
			material.SetPass(pass);
			GL.Begin(5);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.End();
			GL.PopMatrix();
			RenderTexture.active = active;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009198 File Offset: 0x00007398
		public static void Destroy(Object obj)
		{
			if (obj != null)
			{
				Object.Destroy(obj);
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000091A9 File Offset: 0x000073A9
		public static void Dispose()
		{
			GraphicsUtils.Destroy(GraphicsUtils.s_Quad);
		}

		// Token: 0x0400010D RID: 269
		private static Texture2D s_WhiteTexture;

		// Token: 0x0400010E RID: 270
		private static Mesh s_Quad;
	}
}
