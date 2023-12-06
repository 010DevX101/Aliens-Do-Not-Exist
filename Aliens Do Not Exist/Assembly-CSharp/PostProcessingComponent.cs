using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000042 RID: 66
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00008B5D File Offset: 0x00006D5D
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00008B65 File Offset: 0x00006D65
		public T model { get; internal set; }

		// Token: 0x0600016B RID: 363 RVA: 0x00008B6E File Offset: 0x00006D6E
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008B7E File Offset: 0x00006D7E
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
