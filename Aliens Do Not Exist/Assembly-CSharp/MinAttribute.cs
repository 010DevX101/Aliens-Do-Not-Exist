using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200001E RID: 30
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00003C74 File Offset: 0x00001E74
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000091 RID: 145
		public readonly float min;
	}
}
