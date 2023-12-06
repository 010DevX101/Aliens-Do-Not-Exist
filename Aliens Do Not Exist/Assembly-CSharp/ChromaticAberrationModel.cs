using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00007CDF File Offset: 0x00005EDF
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00007CE7 File Offset: 0x00005EE7
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x06000123 RID: 291 RVA: 0x00007CF0 File Offset: 0x00005EF0
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x02000076 RID: 118
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000AAA0 File Offset: 0x00008CA0
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x0400021A RID: 538
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x0400021B RID: 539
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
