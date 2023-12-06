using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007C3A File Offset: 0x00005E3A
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00007C42 File Offset: 0x00005E42
		public BloomModel.Settings settings
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

		// Token: 0x06000119 RID: 281 RVA: 0x00007C4B File Offset: 0x00005E4B
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x0200006F RID: 111
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x1700004B RID: 75
			// (get) Token: 0x060001CA RID: 458 RVA: 0x0000A92C File Offset: 0x00008B2C
			// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000A91E File Offset: 0x00008B1E
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060001CB RID: 459 RVA: 0x0000A93C File Offset: 0x00008B3C
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x040001FC RID: 508
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x040001FD RID: 509
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x040001FE RID: 510
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x040001FF RID: 511
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04000200 RID: 512
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x02000070 RID: 112
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060001CC RID: 460 RVA: 0x0000A98C File Offset: 0x00008B8C
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04000201 RID: 513
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04000202 RID: 514
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x02000071 RID: 113
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060001CD RID: 461 RVA: 0x0000A9B8 File Offset: 0x00008BB8
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000203 RID: 515
			public BloomModel.BloomSettings bloom;

			// Token: 0x04000204 RID: 516
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
