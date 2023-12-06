using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000044 RID: 68
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00008B9B File Offset: 0x00006D9B
		public virtual void Prepare(Material material)
		{
		}
	}
}
