using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000029 RID: 41
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000DC RID: 220 RVA: 0x0000633D File Offset: 0x0000453D
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006370 File Offset: 0x00004570
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006377 File Offset: 0x00004577
		public override DepthTextureMode GetCameraFlags()
		{
			return 1;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000637A File Offset: 0x0000457A
		public override CameraEvent GetCameraEvent()
		{
			return 13;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006380 File Offset: 0x00004580
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color color = GraphicsUtils.isLinearColorSpace ? RenderSettings.fogColor.linear : RenderSettings.fogColor;
			material.SetColor(FogComponent.Uniforms._FogColor, color);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			switch (RenderSettings.fogMode)
			{
			case 1:
				material.EnableKeyword("FOG_LINEAR");
				break;
			case 2:
				material.EnableKeyword("FOG_EXP");
				break;
			case 3:
				material.EnableKeyword("FOG_EXP2");
				break;
			}
			RenderTextureFormat renderTextureFormat = this.context.isHdr ? 9 : 7;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, 1, renderTextureFormat);
			cb.Blit(2, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, 2, material, settings.excludeSkybox ? 1 : 0);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x040000B2 RID: 178
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x0200005A RID: 90
		private static class Uniforms
		{
			// Token: 0x04000168 RID: 360
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x04000169 RID: 361
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x0400016A RID: 362
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x0400016B RID: 363
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x0400016C RID: 364
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
