using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00007E3C File Offset: 0x0000603C
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00007E44 File Offset: 0x00006044
		public GrainModel.Settings settings
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

		// Token: 0x06000140 RID: 320 RVA: 0x00007E4D File Offset: 0x0000604D
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x040000CF RID: 207
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x02000087 RID: 135
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005F RID: 95
			// (get) Token: 0x060001DE RID: 478 RVA: 0x0000B0A0 File Offset: 0x000092A0
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04000268 RID: 616
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04000269 RID: 617
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x0400026A RID: 618
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x0400026B RID: 619
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
