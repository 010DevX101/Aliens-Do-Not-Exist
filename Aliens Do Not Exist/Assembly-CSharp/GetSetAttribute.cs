using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200001D RID: 29
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00003C65 File Offset: 0x00001E65
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x0400008F RID: 143
		public readonly string name;

		// Token: 0x04000090 RID: 144
		public bool dirty;
	}
}
