using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00007BD8 File Offset: 0x00005DD8
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00007BE0 File Offset: 0x00005DE0
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x06000111 RID: 273 RVA: 0x00007BE9 File Offset: 0x00005DE9
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x02000066 RID: 102
		public enum SampleCount
		{
			// Token: 0x040001D7 RID: 471
			Lowest = 3,
			// Token: 0x040001D8 RID: 472
			Low = 6,
			// Token: 0x040001D9 RID: 473
			Medium = 10,
			// Token: 0x040001DA RID: 474
			High = 16
		}

		// Token: 0x02000067 RID: 103
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000047 RID: 71
			// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000A5B4 File Offset: 0x000087B4
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x040001DB RID: 475
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x040001DC RID: 476
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x040001DD RID: 477
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x040001DE RID: 478
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x040001DF RID: 479
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x040001E0 RID: 480
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x040001E1 RID: 481
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
