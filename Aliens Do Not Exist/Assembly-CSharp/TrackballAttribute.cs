using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200001F RID: 31
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00003C83 File Offset: 0x00001E83
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04000092 RID: 146
		public readonly string method;
	}
}
