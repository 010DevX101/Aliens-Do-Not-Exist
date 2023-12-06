using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000025 RID: 37
	public sealed class ColorGradingComponent : PostProcessingComponentRenderTexture<ColorGradingModel>
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004AC8 File Offset: 0x00002CC8
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004AE7 File Offset: 0x00002CE7
		private float StandardIlluminantY(float x)
		{
			return 2.87f * x - 3f * x * x - 0.27509508f;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004B00 File Offset: 0x00002D00
		private Vector3 CIExyToLMS(float x, float y)
		{
			float num = 1f;
			float num2 = num * x / y;
			float num3 = num * (1f - x - y) / y;
			float num4 = 0.7328f * num2 + 0.4296f * num - 0.1624f * num3;
			float num5 = -0.7036f * num2 + 1.6975f * num + 0.0061f * num3;
			float num6 = 0.003f * num2 + 0.0136f * num + 0.9834f * num3;
			return new Vector3(num4, num5, num6);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004B78 File Offset: 0x00002D78
		private Vector3 CalculateColorBalance(float temperature, float tint)
		{
			float num = temperature / 55f;
			float num2 = tint / 55f;
			float x = 0.31271f - num * ((num < 0f) ? 0.1f : 0.05f);
			float y = this.StandardIlluminantY(x) + num2 * 0.05f;
			Vector3 vector;
			vector..ctor(0.949237f, 1.03542f, 1.08728f);
			Vector3 vector2 = this.CIExyToLMS(x, y);
			return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004C14 File Offset: 0x00002E14
		private static Color NormalizeColor(Color c)
		{
			float num = (c.r + c.g + c.b) / 3f;
			if (Mathf.Approximately(num, 0f))
			{
				return new Color(1f, 1f, 1f, c.a);
			}
			Color result = default(Color);
			result.r = c.r / num;
			result.g = c.g / num;
			result.b = c.b / num;
			result.a = c.a;
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004CA7 File Offset: 0x00002EA7
		private static Vector3 ClampVector(Vector3 v, float min, float max)
		{
			return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public static Vector3 GetLiftValue(Color lift)
		{
			Color color = ColorGradingComponent.NormalizeColor(lift);
			float num = (color.r + color.g + color.b) / 3f;
			float num2 = (color.r - num) * 0.1f + lift.a;
			float num3 = (color.g - num) * 0.1f + lift.a;
			float num4 = (color.b - num) * 0.1f + lift.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), -1f, 1f);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004D60 File Offset: 0x00002F60
		public static Vector3 GetGammaValue(Color gamma)
		{
			Color color = ColorGradingComponent.NormalizeColor(gamma);
			float num = (color.r + color.g + color.b) / 3f;
			gamma.a *= ((gamma.a < 0f) ? 0.8f : 5f);
			float num2 = Mathf.Pow(2f, (color.r - num) * 0.5f) + gamma.a;
			float num3 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gamma.a;
			float num4 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gamma.a;
			float num5 = 1f / Mathf.Max(0.01f, num2);
			float num6 = 1f / Mathf.Max(0.01f, num3);
			float num7 = 1f / Mathf.Max(0.01f, num4);
			return ColorGradingComponent.ClampVector(new Vector3(num5, num6, num7), 0f, 5f);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004E64 File Offset: 0x00003064
		public static Vector3 GetGainValue(Color gain)
		{
			Color color = ColorGradingComponent.NormalizeColor(gain);
			float num = (color.r + color.g + color.b) / 3f;
			gain.a *= ((gain.a > 0f) ? 3f : 1f);
			float num2 = Mathf.Pow(2f, (color.r - num) * 0.5f) + gain.a;
			float num3 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gain.a;
			float num4 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gain.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), 0f, 4f);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004F2C File Offset: 0x0000312C
		public static void CalculateLiftGammaGain(Color lift, Color gamma, Color gain, out Vector3 outLift, out Vector3 outGamma, out Vector3 outGain)
		{
			outLift = ColorGradingComponent.GetLiftValue(lift);
			outGamma = ColorGradingComponent.GetGammaValue(gamma);
			outGain = ColorGradingComponent.GetGainValue(gain);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004F54 File Offset: 0x00003154
		public static Vector3 GetSlopeValue(Color slope)
		{
			Color color = ColorGradingComponent.NormalizeColor(slope);
			float num = (color.r + color.g + color.b) / 3f;
			slope.a *= 0.5f;
			float num2 = (color.r - num) * 0.1f + slope.a + 1f;
			float num3 = (color.g - num) * 0.1f + slope.a + 1f;
			float num4 = (color.b - num) * 0.1f + slope.a + 1f;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), 0f, 2f);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004FFC File Offset: 0x000031FC
		public static Vector3 GetPowerValue(Color power)
		{
			Color color = ColorGradingComponent.NormalizeColor(power);
			float num = (color.r + color.g + color.b) / 3f;
			power.a *= 0.5f;
			float num2 = (color.r - num) * 0.1f + power.a + 1f;
			float num3 = (color.g - num) * 0.1f + power.a + 1f;
			float num4 = (color.b - num) * 0.1f + power.a + 1f;
			float num5 = 1f / Mathf.Max(0.01f, num2);
			float num6 = 1f / Mathf.Max(0.01f, num3);
			float num7 = 1f / Mathf.Max(0.01f, num4);
			return ColorGradingComponent.ClampVector(new Vector3(num5, num6, num7), 0.5f, 2.5f);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000050E0 File Offset: 0x000032E0
		public static Vector3 GetOffsetValue(Color offset)
		{
			Color color = ColorGradingComponent.NormalizeColor(offset);
			float num = (color.r + color.g + color.b) / 3f;
			offset.a *= 0.5f;
			float num2 = (color.r - num) * 0.05f + offset.a;
			float num3 = (color.g - num) * 0.05f + offset.a;
			float num4 = (color.b - num) * 0.05f + offset.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), -0.8f, 0.8f);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005176 File Offset: 0x00003376
		public static void CalculateSlopePowerOffset(Color slope, Color power, Color offset, out Vector3 outSlope, out Vector3 outPower, out Vector3 outOffset)
		{
			outSlope = ColorGradingComponent.GetSlopeValue(slope);
			outPower = ColorGradingComponent.GetPowerValue(power);
			outOffset = ColorGradingComponent.GetOffsetValue(offset);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000519E File Offset: 0x0000339E
		private TextureFormat GetCurveFormat()
		{
			if (SystemInfo.SupportsTextureFormat(17))
			{
				return 17;
			}
			return 4;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000051B0 File Offset: 0x000033B0
		private Texture2D GetCurveTexture()
		{
			if (this.m_GradingCurves == null)
			{
				this.m_GradingCurves = new Texture2D(128, 2, this.GetCurveFormat(), false, true)
				{
					name = "Internal Curves Texture",
					hideFlags = 52,
					anisoLevel = 0,
					wrapMode = 1,
					filterMode = 1
				};
			}
			ColorGradingModel.CurvesSettings curves = base.model.settings.curves;
			curves.hueVShue.Cache();
			curves.hueVSsat.Cache();
			for (int i = 0; i < 128; i++)
			{
				float t = (float)i * 0.0078125f;
				float num = curves.hueVShue.Evaluate(t);
				float num2 = curves.hueVSsat.Evaluate(t);
				float num3 = curves.satVSsat.Evaluate(t);
				float num4 = curves.lumVSsat.Evaluate(t);
				this.m_pixels[i] = new Color(num, num2, num3, num4);
				float num5 = curves.master.Evaluate(t);
				float num6 = curves.red.Evaluate(t);
				float num7 = curves.green.Evaluate(t);
				float num8 = curves.blue.Evaluate(t);
				this.m_pixels[i + 128] = new Color(num6, num7, num8, num5);
			}
			this.m_GradingCurves.SetPixels(this.m_pixels);
			this.m_GradingCurves.Apply(false, false);
			return this.m_GradingCurves;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000531C File Offset: 0x0000351C
		private bool IsLogLutValid(RenderTexture lut)
		{
			return lut != null && lut.IsCreated() && lut.height == 32;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000533B File Offset: 0x0000353B
		private RenderTextureFormat GetLutFormat()
		{
			if (SystemInfo.SupportsRenderTextureFormat(2))
			{
				return 2;
			}
			return 0;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005348 File Offset: 0x00003548
		private void GenerateLut()
		{
			ColorGradingModel.Settings settings = base.model.settings;
			if (!this.IsLogLutValid(base.model.bakedLut))
			{
				GraphicsUtils.Destroy(base.model.bakedLut);
				base.model.bakedLut = new RenderTexture(1024, 32, 0, this.GetLutFormat())
				{
					name = "Color Grading Log LUT",
					hideFlags = 52,
					filterMode = 1,
					wrapMode = 1,
					anisoLevel = 0
				};
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Lut Generator");
			material.SetVector(ColorGradingComponent.Uniforms._LutParams, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			material.shaderKeywords = null;
			ColorGradingModel.TonemappingSettings tonemapping = settings.tonemapping;
			ColorGradingModel.Tonemapper tonemapper = tonemapping.tonemapper;
			if (tonemapper != ColorGradingModel.Tonemapper.ACES)
			{
				if (tonemapper == ColorGradingModel.Tonemapper.Neutral)
				{
					material.EnableKeyword("TONEMAPPING_NEUTRAL");
					float num = tonemapping.neutralBlackIn * 20f + 1f;
					float num2 = tonemapping.neutralBlackOut * 10f + 1f;
					float num3 = tonemapping.neutralWhiteIn / 20f;
					float num4 = 1f - tonemapping.neutralWhiteOut / 20f;
					float num5 = num / num2;
					float num6 = num3 / num4;
					float num7 = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, num5));
					float num8 = Mathf.LerpUnclamped(0.01f, 0.24f, num6);
					float num9 = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, num5));
					material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams1, new Vector4(0.2f, num7, num8, num9));
					material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams2, new Vector4(0.02f, 0.3f, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10f));
				}
			}
			else
			{
				material.EnableKeyword("TONEMAPPING_FILMIC");
			}
			material.SetFloat(ColorGradingComponent.Uniforms._HueShift, settings.basic.hueShift / 360f);
			material.SetFloat(ColorGradingComponent.Uniforms._Saturation, settings.basic.saturation);
			material.SetFloat(ColorGradingComponent.Uniforms._Contrast, settings.basic.contrast);
			material.SetVector(ColorGradingComponent.Uniforms._Balance, this.CalculateColorBalance(settings.basic.temperature, settings.basic.tint));
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			ColorGradingComponent.CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, out vector, out vector2, out vector3);
			material.SetVector(ColorGradingComponent.Uniforms._Lift, vector);
			material.SetVector(ColorGradingComponent.Uniforms._InvGamma, vector2);
			material.SetVector(ColorGradingComponent.Uniforms._Gain, vector3);
			Vector3 vector4;
			Vector3 vector5;
			Vector3 vector6;
			ColorGradingComponent.CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, out vector4, out vector5, out vector6);
			material.SetVector(ColorGradingComponent.Uniforms._Slope, vector4);
			material.SetVector(ColorGradingComponent.Uniforms._Power, vector5);
			material.SetVector(ColorGradingComponent.Uniforms._Offset, vector6);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerRed, settings.channelMixer.red);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerGreen, settings.channelMixer.green);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerBlue, settings.channelMixer.blue);
			material.SetTexture(ColorGradingComponent.Uniforms._Curves, this.GetCurveTexture());
			Graphics.Blit(null, base.model.bakedLut, material, 0);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000056F4 File Offset: 0x000038F4
		public override void Prepare(Material uberMaterial)
		{
			if (base.model.isDirty || !this.IsLogLutValid(base.model.bakedLut))
			{
				this.GenerateLut();
				base.model.isDirty = false;
			}
			uberMaterial.EnableKeyword(this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) ? "COLOR_GRADING_LOG_VIEW" : "COLOR_GRADING");
			RenderTexture bakedLut = base.model.bakedLut;
			uberMaterial.SetTexture(ColorGradingComponent.Uniforms._LogLut, bakedLut);
			uberMaterial.SetVector(ColorGradingComponent.Uniforms._LogLut_Params, new Vector3(1f / (float)bakedLut.width, 1f / (float)bakedLut.height, (float)bakedLut.height - 1f));
			float num = Mathf.Exp(base.model.settings.basic.postExposure * 0.6931472f);
			uberMaterial.SetFloat(ColorGradingComponent.Uniforms._ExposureEV, num);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000057E0 File Offset: 0x000039E0
		public void OnGUI()
		{
			RenderTexture bakedLut = base.model.bakedLut;
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)bakedLut.width, (float)bakedLut.height), bakedLut);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005837 File Offset: 0x00003A37
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GradingCurves);
			GraphicsUtils.Destroy(base.model.bakedLut);
			this.m_GradingCurves = null;
			base.model.bakedLut = null;
		}

		// Token: 0x0400009C RID: 156
		private const int k_InternalLogLutSize = 32;

		// Token: 0x0400009D RID: 157
		private const int k_CurvePrecision = 128;

		// Token: 0x0400009E RID: 158
		private const float k_CurveStep = 0.0078125f;

		// Token: 0x0400009F RID: 159
		private Texture2D m_GradingCurves;

		// Token: 0x040000A0 RID: 160
		private Color[] m_pixels = new Color[256];

		// Token: 0x02000056 RID: 86
		private static class Uniforms
		{
			// Token: 0x04000141 RID: 321
			internal static readonly int _LutParams = Shader.PropertyToID("_LutParams");

			// Token: 0x04000142 RID: 322
			internal static readonly int _NeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");

			// Token: 0x04000143 RID: 323
			internal static readonly int _NeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");

			// Token: 0x04000144 RID: 324
			internal static readonly int _HueShift = Shader.PropertyToID("_HueShift");

			// Token: 0x04000145 RID: 325
			internal static readonly int _Saturation = Shader.PropertyToID("_Saturation");

			// Token: 0x04000146 RID: 326
			internal static readonly int _Contrast = Shader.PropertyToID("_Contrast");

			// Token: 0x04000147 RID: 327
			internal static readonly int _Balance = Shader.PropertyToID("_Balance");

			// Token: 0x04000148 RID: 328
			internal static readonly int _Lift = Shader.PropertyToID("_Lift");

			// Token: 0x04000149 RID: 329
			internal static readonly int _InvGamma = Shader.PropertyToID("_InvGamma");

			// Token: 0x0400014A RID: 330
			internal static readonly int _Gain = Shader.PropertyToID("_Gain");

			// Token: 0x0400014B RID: 331
			internal static readonly int _Slope = Shader.PropertyToID("_Slope");

			// Token: 0x0400014C RID: 332
			internal static readonly int _Power = Shader.PropertyToID("_Power");

			// Token: 0x0400014D RID: 333
			internal static readonly int _Offset = Shader.PropertyToID("_Offset");

			// Token: 0x0400014E RID: 334
			internal static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

			// Token: 0x0400014F RID: 335
			internal static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

			// Token: 0x04000150 RID: 336
			internal static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

			// Token: 0x04000151 RID: 337
			internal static readonly int _Curves = Shader.PropertyToID("_Curves");

			// Token: 0x04000152 RID: 338
			internal static readonly int _LogLut = Shader.PropertyToID("_LogLut");

			// Token: 0x04000153 RID: 339
			internal static readonly int _LogLut_Params = Shader.PropertyToID("_LogLut_Params");

			// Token: 0x04000154 RID: 340
			internal static readonly int _ExposureEV = Shader.PropertyToID("_ExposureEV");
		}
	}
}
