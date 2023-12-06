using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200002F RID: 47
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00007950 File Offset: 0x00005B50
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000079C0 File Offset: 0x00005BC0
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007A44 File Offset: 0x00005C44
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height), settings.lut);
		}

		// Token: 0x02000064 RID: 100
		private static class Uniforms
		{
			// Token: 0x040001CF RID: 463
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x040001D0 RID: 464
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
