using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000041 RID: 65
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00008B4E File Offset: 0x00006D4E
		public virtual DepthTextureMode GetCameraFlags()
		{
			return 0;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000164 RID: 356
		public abstract bool active { get; }

		// Token: 0x06000165 RID: 357 RVA: 0x00008B51 File Offset: 0x00006D51
		public virtual void OnEnable()
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00008B53 File Offset: 0x00006D53
		public virtual void OnDisable()
		{
		}

		// Token: 0x06000167 RID: 359
		public abstract PostProcessingModel GetModel();

		// Token: 0x040000F1 RID: 241
		public PostProcessingContext context;
	}
}
