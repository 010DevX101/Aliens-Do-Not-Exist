using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00007E0B File Offset: 0x0000600B
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00007E13 File Offset: 0x00006013
		public FogModel.Settings settings
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

		// Token: 0x0600013C RID: 316 RVA: 0x00007E1C File Offset: 0x0000601C
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x040000CE RID: 206
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x02000086 RID: 134
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005E RID: 94
			// (get) Token: 0x060001DD RID: 477 RVA: 0x0000B080 File Offset: 0x00009280
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04000267 RID: 615
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
