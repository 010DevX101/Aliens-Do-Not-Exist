using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000027 RID: 39
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005CBE File Offset: 0x00003EBE
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005CDD File Offset: 0x00003EDD
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005CE8 File Offset: 0x00003EE8
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i.ToString());
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005D30 File Offset: 0x00003F30
		public override void Prepare(Material uberMaterial)
		{
			int num = this.textureIndex + 1;
			this.textureIndex = num;
			if (num >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x040000A4 RID: 164
		private Texture2D[] noiseTextures;

		// Token: 0x040000A5 RID: 165
		private int textureIndex;

		// Token: 0x040000A6 RID: 166
		private const int k_TextureCount = 64;

		// Token: 0x02000058 RID: 88
		private static class Uniforms
		{
			// Token: 0x04000160 RID: 352
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x04000161 RID: 353
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
