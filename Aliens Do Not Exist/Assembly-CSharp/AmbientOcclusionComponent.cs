using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000021 RID: 33
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003C9C File Offset: 0x00001E9C
		private AmbientOcclusionComponent.OcclusionSource occlusionSource
		{
			get
			{
				if (this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)
				{
					return AmbientOcclusionComponent.OcclusionSource.GBuffer;
				}
				if (base.model.settings.highPrecision && (!this.context.isGBufferAvailable || base.model.settings.forceForwardCompatibility))
				{
					return AmbientOcclusionComponent.OcclusionSource.DepthTexture;
				}
				return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003D00 File Offset: 0x00001F00
		private bool ambientOnlySupported
		{
			get
			{
				return this.context.isHdr && base.model.settings.ambientOnly && this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003D4E File Offset: 0x00001F4E
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003D84 File Offset: 0x00001F84
		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = 0;
			if (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.DepthTexture)
			{
				depthTextureMode |= 1;
			}
			if (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer)
			{
				depthTextureMode |= 2;
			}
			return depthTextureMode;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003DAD File Offset: 0x00001FAD
		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public override CameraEvent GetCameraEvent()
		{
			if (!this.ambientOnlySupported || this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				return 12;
			}
			return 21;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003DDC File Offset: 0x00001FDC
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material2.shaderKeywords = null;
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, settings.downsampling ? 0.5f : 1f);
			material2.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, (int)settings.sampleCount);
			if (!this.context.isGBufferAvailable && RenderSettings.fog)
			{
				material2.SetVector(AmbientOcclusionComponent.Uniforms._FogParams, new Vector3(RenderSettings.fogDensity, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance));
				switch (RenderSettings.fogMode)
				{
				case 1:
					material2.EnableKeyword("FOG_LINEAR");
					break;
				case 2:
					material2.EnableKeyword("FOG_EXP");
					break;
				case 3:
					material2.EnableKeyword("FOG_EXP2");
					break;
				}
			}
			else
			{
				material2.EnableKeyword("FOG_OFF");
			}
			int width = this.context.width;
			int height = this.context.height;
			int num = settings.downsampling ? 2 : 1;
			int num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(num2, width / num, height / num, 0, 1, 0, 1);
			cb.Blit(null, num2, material2, (int)this.occlusionSource);
			int occlusionTexture = AmbientOcclusionComponent.Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, 1, 0, 1);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
			cb.Blit(num2, occlusionTexture, material2, (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.GBuffer) ? 4 : 3);
			cb.ReleaseTemporaryRT(num2);
			num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(num2, width, height, 0, 1, 0, 1);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, num2, material2, 5);
			cb.ReleaseTemporaryRT(occlusionTexture);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
				cb.Blit(num2, 2, material2, 8);
				this.context.Interrupt();
			}
			else if (this.ambientOnlySupported)
			{
				cb.SetRenderTarget(this.m_MRT, 2);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7);
			}
			else
			{
				RenderTextureFormat renderTextureFormat = this.context.isHdr ? 9 : 7;
				int tempRT = AmbientOcclusionComponent.Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 1, renderTextureFormat);
				cb.Blit(2, tempRT, material, 0);
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, 2, material2, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(num2);
		}

		// Token: 0x04000093 RID: 147
		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x04000094 RID: 148
		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		// Token: 0x04000095 RID: 149
		private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[]
		{
			10,
			2
		};

		// Token: 0x0200004F RID: 79
		private static class Uniforms
		{
			// Token: 0x04000117 RID: 279
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04000118 RID: 280
			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			// Token: 0x04000119 RID: 281
			internal static readonly int _FogParams = Shader.PropertyToID("_FogParams");

			// Token: 0x0400011A RID: 282
			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			// Token: 0x0400011B RID: 283
			internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			// Token: 0x0400011C RID: 284
			internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			// Token: 0x0400011D RID: 285
			internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			// Token: 0x0400011E RID: 286
			internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			// Token: 0x0400011F RID: 287
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000120 RID: 288
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		// Token: 0x02000050 RID: 80
		private enum OcclusionSource
		{
			// Token: 0x04000122 RID: 290
			DepthTexture,
			// Token: 0x04000123 RID: 291
			DepthNormalsTexture,
			// Token: 0x04000124 RID: 292
			GBuffer
		}
	}
}
