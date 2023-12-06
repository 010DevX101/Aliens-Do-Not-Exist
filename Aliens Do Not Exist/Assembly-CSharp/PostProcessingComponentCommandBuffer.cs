using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000043 RID: 67
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x0600016E RID: 366
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x0600016F RID: 367
		public abstract string GetName();

		// Token: 0x06000170 RID: 368
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
