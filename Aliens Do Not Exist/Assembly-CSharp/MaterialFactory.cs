using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200004A RID: 74
	public sealed class MaterialFactory : IDisposable
	{
		// Token: 0x0600018F RID: 399 RVA: 0x000091B5 File Offset: 0x000073B5
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000091C8 File Offset: 0x000073C8
		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = 52
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009244 File Offset: 0x00007444
		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				GraphicsUtils.Destroy(keyValuePair.Value);
			}
			this.m_Materials.Clear();
		}

		// Token: 0x0400010F RID: 271
		private Dictionary<string, Material> m_Materials;
	}
}
