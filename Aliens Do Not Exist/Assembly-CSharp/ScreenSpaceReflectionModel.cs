using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007E9E File Offset: 0x0000609E
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00007EA6 File Offset: 0x000060A6
		public ScreenSpaceReflectionModel.Settings settings
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

		// Token: 0x06000148 RID: 328 RVA: 0x00007EAF File Offset: 0x000060AF
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x02000089 RID: 137
		public enum SSRResolution
		{
			// Token: 0x04000270 RID: 624
			High,
			// Token: 0x04000271 RID: 625
			Low = 2
		}

		// Token: 0x0200008A RID: 138
		public enum SSRReflectionBlendType
		{
			// Token: 0x04000273 RID: 627
			PhysicallyBased,
			// Token: 0x04000274 RID: 628
			Additive
		}

		// Token: 0x0200008B RID: 139
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04000275 RID: 629
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04000276 RID: 630
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04000277 RID: 631
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04000278 RID: 632
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x0200008C RID: 140
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x04000279 RID: 633
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x0400027A RID: 634
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x0400027B RID: 635
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x0400027C RID: 636
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x0400027D RID: 637
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x0400027E RID: 638
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x0400027F RID: 639
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x04000280 RID: 640
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x0200008D RID: 141
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04000281 RID: 641
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x0200008E RID: 142
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000061 RID: 97
			// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000B11C File Offset: 0x0000931C
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04000282 RID: 642
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04000283 RID: 643
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04000284 RID: 644
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
