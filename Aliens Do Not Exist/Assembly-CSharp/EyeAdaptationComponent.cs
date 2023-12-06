using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000028 RID: 40
	public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005DDA File Offset: 0x00003FDA
		public override bool active
		{
			get
			{
				return base.model.enabled && SystemInfo.supportsComputeShaders && !this.context.interrupted;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005E00 File Offset: 0x00004000
		public void ResetHistory()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005E09 File Offset: 0x00004009
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005E14 File Offset: 0x00004014
		public override void OnDisable()
		{
			RenderTexture[] autoExposurePool = this.m_AutoExposurePool;
			for (int i = 0; i < autoExposurePool.Length; i++)
			{
				GraphicsUtils.Destroy(autoExposurePool[i]);
			}
			if (this.m_HistogramBuffer != null)
			{
				this.m_HistogramBuffer.Release();
			}
			this.m_HistogramBuffer = null;
			if (this.m_DebugHistogram != null)
			{
				this.m_DebugHistogram.Release();
			}
			this.m_DebugHistogram = null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005E78 File Offset: 0x00004078
		private Vector4 GetHistogramScaleOffsetRes()
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			float num = (float)(settings.logMax - settings.logMin);
			float num2 = 1f / num;
			float num3 = (float)(-(float)settings.logMin) * num2;
			return new Vector4(num2, num3, Mathf.Floor((float)this.context.width / 2f), Mathf.Floor((float)this.context.height / 2f));
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005EE8 File Offset: 0x000040E8
		public Texture Prepare(RenderTexture source, Material uberMaterial)
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			if (this.m_EyeCompute == null)
			{
				this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
			material.shaderKeywords = null;
			if (this.m_HistogramBuffer == null)
			{
				this.m_HistogramBuffer = new ComputeBuffer(64, 4);
			}
			if (EyeAdaptationComponent.s_EmptyHistogramBuffer == null)
			{
				EyeAdaptationComponent.s_EmptyHistogramBuffer = new uint[64];
			}
			Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
			RenderTexture renderTexture = this.context.renderTextureFactory.Get((int)histogramScaleOffsetRes.z, (int)histogramScaleOffsetRes.w, 0, source.format, 0, 1, 1, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture);
			if (this.m_AutoExposurePool[0] == null || !this.m_AutoExposurePool[0].IsCreated())
			{
				this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, 14);
			}
			if (this.m_AutoExposurePool[1] == null || !this.m_AutoExposurePool[1].IsCreated())
			{
				this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, 14);
			}
			this.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer);
			int num = this.m_EyeCompute.FindKernel("KEyeHistogram");
			this.m_EyeCompute.SetBuffer(num, "_Histogram", this.m_HistogramBuffer);
			this.m_EyeCompute.SetTexture(num, "_Source", renderTexture);
			this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
			this.m_EyeCompute.Dispatch(num, Mathf.CeilToInt((float)renderTexture.width / 16f), Mathf.CeilToInt((float)renderTexture.height / 16f), 1);
			this.context.renderTextureFactory.Release(renderTexture);
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
			material.SetBuffer("_Histogram", this.m_HistogramBuffer);
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, Mathf.Exp(settings.minLuminance * 0.6931472f), Mathf.Exp(settings.maxLuminance * 0.6931472f)));
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.keyValue);
			if (settings.dynamicKeyValue)
			{
				material.EnableKeyword("AUTO_KEY_VALUE");
			}
			if (this.m_FirstFrame || !Application.isPlaying)
			{
				this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
				Graphics.Blit(null, this.m_CurrentAutoExposure, material, 1);
				Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
			}
			else
			{
				int num2 = this.m_AutoExposurePingPing;
				Texture texture = this.m_AutoExposurePool[++num2 % 2];
				RenderTexture renderTexture2 = this.m_AutoExposurePool[++num2 % 2];
				Graphics.Blit(texture, renderTexture2, material, (int)settings.adaptationType);
				this.m_AutoExposurePingPing = (num2 + 1) % 2;
				this.m_CurrentAutoExposure = renderTexture2;
			}
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
				{
					this.m_DebugHistogram = new RenderTexture(256, 128, 0, 0)
					{
						filterMode = 0,
						wrapMode = 1
					};
				}
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, (float)this.m_DebugHistogram.width);
				Graphics.Blit(null, this.m_DebugHistogram, material, 2);
			}
			this.m_FirstFrame = false;
			return this.m_CurrentAutoExposure;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000062AC File Offset: 0x000044AC
		public void OnGUI()
		{
			if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
			{
				return;
			}
			GUI.DrawTexture(new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)this.m_DebugHistogram.width, (float)this.m_DebugHistogram.height), this.m_DebugHistogram);
		}

		// Token: 0x040000A7 RID: 167
		private ComputeShader m_EyeCompute;

		// Token: 0x040000A8 RID: 168
		private ComputeBuffer m_HistogramBuffer;

		// Token: 0x040000A9 RID: 169
		private readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];

		// Token: 0x040000AA RID: 170
		private int m_AutoExposurePingPing;

		// Token: 0x040000AB RID: 171
		private RenderTexture m_CurrentAutoExposure;

		// Token: 0x040000AC RID: 172
		private RenderTexture m_DebugHistogram;

		// Token: 0x040000AD RID: 173
		private static uint[] s_EmptyHistogramBuffer;

		// Token: 0x040000AE RID: 174
		private bool m_FirstFrame = true;

		// Token: 0x040000AF RID: 175
		private const int k_HistogramBins = 64;

		// Token: 0x040000B0 RID: 176
		private const int k_HistogramThreadX = 16;

		// Token: 0x040000B1 RID: 177
		private const int k_HistogramThreadY = 16;

		// Token: 0x02000059 RID: 89
		private static class Uniforms
		{
			// Token: 0x04000162 RID: 354
			internal static readonly int _Params = Shader.PropertyToID("_Params");

			// Token: 0x04000163 RID: 355
			internal static readonly int _Speed = Shader.PropertyToID("_Speed");

			// Token: 0x04000164 RID: 356
			internal static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

			// Token: 0x04000165 RID: 357
			internal static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");

			// Token: 0x04000166 RID: 358
			internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			// Token: 0x04000167 RID: 359
			internal static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
		}
	}
}
