using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00007E6D File Offset: 0x0000606D
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00007E75 File Offset: 0x00006075
		public MotionBlurModel.Settings settings
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

		// Token: 0x06000144 RID: 324 RVA: 0x00007E7E File Offset: 0x0000607E
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x040000D0 RID: 208
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x02000088 RID: 136
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000060 RID: 96
			// (get) Token: 0x060001DF RID: 479 RVA: 0x0000B0E4 File Offset: 0x000092E4
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x0400026C RID: 620
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x0400026D RID: 621
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x0400026E RID: 622
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
