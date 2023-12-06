using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00008C31 File Offset: 0x00006E31
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00008C39 File Offset: 0x00006E39
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x06000180 RID: 384
		public abstract void Reset();

		// Token: 0x06000181 RID: 385 RVA: 0x00008C4B File Offset: 0x00006E4B
		public virtual void OnValidate()
		{
		}

		// Token: 0x040000F8 RID: 248
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
