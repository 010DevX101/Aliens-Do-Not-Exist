using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200004B RID: 75
	public sealed class RenderTextureFactory : IDisposable
	{
		// Token: 0x06000192 RID: 402 RVA: 0x00009287 File Offset: 0x00007487
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000929C File Offset: 0x0000749C
		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, baseRenderTexture.sRGB ? 2 : 1, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000092E4 File Offset: 0x000074E4
		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = 2, RenderTextureReadWrite rw = 0, FilterMode filterMode = 1, TextureWrapMode wrapMode = 1, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format, rw);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009324 File Offset: 0x00007524
		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009364 File Offset: 0x00007564
		public void ReleaseAll()
		{
			foreach (RenderTexture renderTexture in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000939F File Offset: 0x0000759F
		public void Dispose()
		{
			this.ReleaseAll();
		}

		// Token: 0x04000110 RID: 272
		private HashSet<RenderTexture> m_TemporaryRTs;
	}
}
