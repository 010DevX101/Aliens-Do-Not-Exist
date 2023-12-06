using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00007C09 File Offset: 0x00005E09
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00007C11 File Offset: 0x00005E11
		public AntialiasingModel.Settings settings
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

		// Token: 0x06000115 RID: 277 RVA: 0x00007C1A File Offset: 0x00005E1A
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x02000068 RID: 104
		public enum Method
		{
			// Token: 0x040001E3 RID: 483
			Fxaa,
			// Token: 0x040001E4 RID: 484
			Taa
		}

		// Token: 0x02000069 RID: 105
		public enum FxaaPreset
		{
			// Token: 0x040001E6 RID: 486
			ExtremePerformance,
			// Token: 0x040001E7 RID: 487
			Performance,
			// Token: 0x040001E8 RID: 488
			Default,
			// Token: 0x040001E9 RID: 489
			Quality,
			// Token: 0x040001EA RID: 490
			ExtremeQuality
		}

		// Token: 0x0200006A RID: 106
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x040001EB RID: 491
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x040001EC RID: 492
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x040001ED RID: 493
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x040001EE RID: 494
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x0200006B RID: 107
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x040001EF RID: 495
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x040001F0 RID: 496
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x040001F1 RID: 497
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x040001F2 RID: 498
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x040001F3 RID: 499
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x0200006C RID: 108
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x17000048 RID: 72
			// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000A880 File Offset: 0x00008A80
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x040001F4 RID: 500
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x0200006D RID: 109
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000A8A0 File Offset: 0x00008AA0
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x040001F5 RID: 501
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x040001F6 RID: 502
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x040001F7 RID: 503
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x040001F8 RID: 504
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x0200006E RID: 110
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700004A RID: 74
			// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000A8E8 File Offset: 0x00008AE8
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x040001F9 RID: 505
			public AntialiasingModel.Method method;

			// Token: 0x040001FA RID: 506
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x040001FB RID: 507
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}
