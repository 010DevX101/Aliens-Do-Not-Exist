using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200002B RID: 43
	public sealed class GrainComponent : PostProcessingComponentRenderTexture<GrainModel>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000065BE File Offset: 0x000047BE
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && SystemInfo.SupportsRenderTextureFormat(2) && !this.context.interrupted;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000065FC File Offset: 0x000047FC
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GrainLookupRT);
			this.m_GrainLookupRT = null;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006610 File Offset: 0x00004810
		public override void Prepare(Material uberMaterial)
		{
			GrainModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("GRAIN");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float value = Random.value;
			float value2 = Random.value;
			if (this.m_GrainLookupRT == null || !this.m_GrainLookupRT.IsCreated())
			{
				GraphicsUtils.Destroy(this.m_GrainLookupRT);
				this.m_GrainLookupRT = new RenderTexture(192, 192, 0, 2)
				{
					filterMode = 1,
					wrapMode = 0,
					anisoLevel = 0,
					name = "Grain Lookup Texture"
				};
				this.m_GrainLookupRT.Create();
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Grain Generator");
			material.SetFloat(GrainComponent.Uniforms._Phase, realtimeSinceStartup / 20f);
			Graphics.Blit(null, this.m_GrainLookupRT, material, settings.colored ? 1 : 0);
			uberMaterial.SetTexture(GrainComponent.Uniforms._GrainTex, this.m_GrainLookupRT);
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params2, new Vector4((float)this.context.width / (float)this.m_GrainLookupRT.width / settings.size, (float)this.context.height / (float)this.m_GrainLookupRT.height / settings.size, value, value2));
		}

		// Token: 0x040000B3 RID: 179
		private RenderTexture m_GrainLookupRT;

		// Token: 0x0200005C RID: 92
		private static class Uniforms
		{
			// Token: 0x0400016F RID: 367
			internal static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");

			// Token: 0x04000170 RID: 368
			internal static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");

			// Token: 0x04000171 RID: 369
			internal static readonly int _GrainTex = Shader.PropertyToID("_GrainTex");

			// Token: 0x04000172 RID: 370
			internal static readonly int _Phase = Shader.PropertyToID("_Phase");
		}
	}
}
