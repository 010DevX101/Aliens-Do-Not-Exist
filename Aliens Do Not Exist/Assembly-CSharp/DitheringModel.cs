using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00007DA9 File Offset: 0x00005FA9
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00007DB1 File Offset: 0x00005FB1
		public DitheringModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007DBA File Offset: 0x00005FBA
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x040000CC RID: 204
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x02000083 RID: 131
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005C RID: 92
			// (get) Token: 0x060001DB RID: 475 RVA: 0x0000AFDC File Offset: 0x000091DC
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
