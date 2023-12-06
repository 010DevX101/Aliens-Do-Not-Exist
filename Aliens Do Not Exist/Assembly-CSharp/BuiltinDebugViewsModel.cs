using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00007C6B File Offset: 0x00005E6B
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00007C73 File Offset: 0x00005E73
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00007C7C File Offset: 0x00005E7C
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007CAF File Offset: 0x00005EAF
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007CBC File Offset: 0x00005EBC
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x02000072 RID: 114
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060001CE RID: 462 RVA: 0x0000A9E8 File Offset: 0x00008BE8
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x04000205 RID: 517
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x02000073 RID: 115
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060001CF RID: 463 RVA: 0x0000AA0C File Offset: 0x00008C0C
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x04000206 RID: 518
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x04000207 RID: 519
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x04000208 RID: 520
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x04000209 RID: 521
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x0400020A RID: 522
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x0400020B RID: 523
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x02000074 RID: 116
		public enum Mode
		{
			// Token: 0x0400020D RID: 525
			None,
			// Token: 0x0400020E RID: 526
			Depth,
			// Token: 0x0400020F RID: 527
			Normals,
			// Token: 0x04000210 RID: 528
			MotionVectors,
			// Token: 0x04000211 RID: 529
			AmbientOcclusion,
			// Token: 0x04000212 RID: 530
			EyeAdaptation,
			// Token: 0x04000213 RID: 531
			FocusPlane,
			// Token: 0x04000214 RID: 532
			PreGradingLog,
			// Token: 0x04000215 RID: 533
			LogLut,
			// Token: 0x04000216 RID: 534
			UserLut
		}

		// Token: 0x02000075 RID: 117
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000AA68 File Offset: 0x00008C68
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000217 RID: 535
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x04000218 RID: 536
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x04000219 RID: 537
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
