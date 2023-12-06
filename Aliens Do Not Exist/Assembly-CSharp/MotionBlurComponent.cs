using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200002C RID: 44
	public sealed class MotionBlurComponent : PostProcessingComponentCommandBuffer<MotionBlurModel>
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00006785 File Offset: 0x00004985
		public MotionBlurComponent.ReconstructionFilter reconstructionFilter
		{
			get
			{
				if (this.m_ReconstructionFilter == null)
				{
					this.m_ReconstructionFilter = new MotionBlurComponent.ReconstructionFilter();
				}
				return this.m_ReconstructionFilter;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000067A0 File Offset: 0x000049A0
		public MotionBlurComponent.FrameBlendingFilter frameBlendingFilter
		{
			get
			{
				if (this.m_FrameBlendingFilter == null)
				{
					this.m_FrameBlendingFilter = new MotionBlurComponent.FrameBlendingFilter();
				}
				return this.m_FrameBlendingFilter;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000067BC File Offset: 0x000049BC
		public override bool active
		{
			get
			{
				MotionBlurModel.Settings settings = base.model.settings;
				return base.model.enabled && ((settings.shutterAngle > 0f && this.reconstructionFilter.IsSupported()) || settings.frameBlending > 0f) && SystemInfo.graphicsDeviceType != 8 && !this.context.interrupted;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006821 File Offset: 0x00004A21
		public override string GetName()
		{
			return "Motion Blur";
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006828 File Offset: 0x00004A28
		public void ResetHistory()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
			this.m_FrameBlendingFilter = null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006844 File Offset: 0x00004A44
		public override DepthTextureMode GetCameraFlags()
		{
			return 5;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006847 File Offset: 0x00004A47
		public override CameraEvent GetCameraEvent()
		{
			return 18;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000684B File Offset: 0x00004A4B
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006854 File Offset: 0x00004A54
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			if (this.m_FirstFrame)
			{
				this.m_FirstFrame = false;
				return;
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Motion Blur");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			MotionBlurModel.Settings settings = base.model.settings;
			RenderTextureFormat renderTextureFormat = this.context.isHdr ? 9 : 7;
			int tempRT = MotionBlurComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 0, renderTextureFormat);
			if (settings.shutterAngle > 0f && settings.frameBlending > 0f)
			{
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, 2, tempRT, material);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, 2, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			else if (settings.shutterAngle > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, 2);
				cb.Blit(2, tempRT, material2, 0);
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, tempRT, 2, material);
			}
			else if (settings.frameBlending > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, 2);
				cb.Blit(2, tempRT, material2, 0);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, 2, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			cb.ReleaseTemporaryRT(tempRT);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006A4B File Offset: 0x00004C4B
		public override void OnDisable()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
		}

		// Token: 0x040000B4 RID: 180
		private MotionBlurComponent.ReconstructionFilter m_ReconstructionFilter;

		// Token: 0x040000B5 RID: 181
		private MotionBlurComponent.FrameBlendingFilter m_FrameBlendingFilter;

		// Token: 0x040000B6 RID: 182
		private bool m_FirstFrame = true;

		// Token: 0x0200005D RID: 93
		private static class Uniforms
		{
			// Token: 0x04000173 RID: 371
			internal static readonly int _VelocityScale = Shader.PropertyToID("_VelocityScale");

			// Token: 0x04000174 RID: 372
			internal static readonly int _MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");

			// Token: 0x04000175 RID: 373
			internal static readonly int _RcpMaxBlurRadius = Shader.PropertyToID("_RcpMaxBlurRadius");

			// Token: 0x04000176 RID: 374
			internal static readonly int _VelocityTex = Shader.PropertyToID("_VelocityTex");

			// Token: 0x04000177 RID: 375
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000178 RID: 376
			internal static readonly int _Tile2RT = Shader.PropertyToID("_Tile2RT");

			// Token: 0x04000179 RID: 377
			internal static readonly int _Tile4RT = Shader.PropertyToID("_Tile4RT");

			// Token: 0x0400017A RID: 378
			internal static readonly int _Tile8RT = Shader.PropertyToID("_Tile8RT");

			// Token: 0x0400017B RID: 379
			internal static readonly int _TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");

			// Token: 0x0400017C RID: 380
			internal static readonly int _TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");

			// Token: 0x0400017D RID: 381
			internal static readonly int _TileVRT = Shader.PropertyToID("_TileVRT");

			// Token: 0x0400017E RID: 382
			internal static readonly int _NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");

			// Token: 0x0400017F RID: 383
			internal static readonly int _LoopCount = Shader.PropertyToID("_LoopCount");

			// Token: 0x04000180 RID: 384
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04000181 RID: 385
			internal static readonly int _History1LumaTex = Shader.PropertyToID("_History1LumaTex");

			// Token: 0x04000182 RID: 386
			internal static readonly int _History2LumaTex = Shader.PropertyToID("_History2LumaTex");

			// Token: 0x04000183 RID: 387
			internal static readonly int _History3LumaTex = Shader.PropertyToID("_History3LumaTex");

			// Token: 0x04000184 RID: 388
			internal static readonly int _History4LumaTex = Shader.PropertyToID("_History4LumaTex");

			// Token: 0x04000185 RID: 389
			internal static readonly int _History1ChromaTex = Shader.PropertyToID("_History1ChromaTex");

			// Token: 0x04000186 RID: 390
			internal static readonly int _History2ChromaTex = Shader.PropertyToID("_History2ChromaTex");

			// Token: 0x04000187 RID: 391
			internal static readonly int _History3ChromaTex = Shader.PropertyToID("_History3ChromaTex");

			// Token: 0x04000188 RID: 392
			internal static readonly int _History4ChromaTex = Shader.PropertyToID("_History4ChromaTex");

			// Token: 0x04000189 RID: 393
			internal static readonly int _History1Weight = Shader.PropertyToID("_History1Weight");

			// Token: 0x0400018A RID: 394
			internal static readonly int _History2Weight = Shader.PropertyToID("_History2Weight");

			// Token: 0x0400018B RID: 395
			internal static readonly int _History3Weight = Shader.PropertyToID("_History3Weight");

			// Token: 0x0400018C RID: 396
			internal static readonly int _History4Weight = Shader.PropertyToID("_History4Weight");
		}

		// Token: 0x0200005E RID: 94
		private enum Pass
		{
			// Token: 0x0400018E RID: 398
			VelocitySetup,
			// Token: 0x0400018F RID: 399
			TileMax1,
			// Token: 0x04000190 RID: 400
			TileMax2,
			// Token: 0x04000191 RID: 401
			TileMaxV,
			// Token: 0x04000192 RID: 402
			NeighborMax,
			// Token: 0x04000193 RID: 403
			Reconstruction,
			// Token: 0x04000194 RID: 404
			FrameCompression,
			// Token: 0x04000195 RID: 405
			FrameBlendingChroma,
			// Token: 0x04000196 RID: 406
			FrameBlendingRaw
		}

		// Token: 0x0200005F RID: 95
		public class ReconstructionFilter
		{
			// Token: 0x060001B4 RID: 436 RVA: 0x00009CF7 File Offset: 0x00007EF7
			public ReconstructionFilter()
			{
				this.CheckTextureFormatSupport();
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x00009D14 File Offset: 0x00007F14
			private void CheckTextureFormatSupport()
			{
				if (!SystemInfo.SupportsRenderTextureFormat(this.m_PackedRTFormat))
				{
					this.m_PackedRTFormat = 0;
				}
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x00009D2A File Offset: 0x00007F2A
			public bool IsSupported()
			{
				return SystemInfo.supportsMotionVectors;
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x00009D34 File Offset: 0x00007F34
			public void ProcessImage(PostProcessingContext context, CommandBuffer cb, ref MotionBlurModel.Settings settings, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				int num = (int)(5f * (float)context.height / 100f);
				int num2 = ((num - 1) / 8 + 1) * 8;
				float num3 = settings.shutterAngle / 360f;
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float)num);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._RcpMaxBlurRadius, 1f / (float)num);
				int velocityTex = MotionBlurComponent.Uniforms._VelocityTex;
				cb.GetTemporaryRT(velocityTex, context.width, context.height, 0, 0, this.m_PackedRTFormat, 1);
				cb.Blit(null, velocityTex, material, 0);
				int tile2RT = MotionBlurComponent.Uniforms._Tile2RT;
				cb.GetTemporaryRT(tile2RT, context.width / 2, context.height / 2, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, velocityTex);
				cb.Blit(velocityTex, tile2RT, material, 1);
				int tile4RT = MotionBlurComponent.Uniforms._Tile4RT;
				cb.GetTemporaryRT(tile4RT, context.width / 4, context.height / 4, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile2RT);
				cb.Blit(tile2RT, tile4RT, material, 2);
				cb.ReleaseTemporaryRT(tile2RT);
				int tile8RT = MotionBlurComponent.Uniforms._Tile8RT;
				cb.GetTemporaryRT(tile8RT, context.width / 8, context.height / 8, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile4RT);
				cb.Blit(tile4RT, tile8RT, material, 2);
				cb.ReleaseTemporaryRT(tile4RT);
				Vector2 vector = Vector2.one * ((float)num2 / 8f - 1f) * -0.5f;
				cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, (float)((int)((float)num2 / 8f)));
				int tileVRT = MotionBlurComponent.Uniforms._TileVRT;
				cb.GetTemporaryRT(tileVRT, context.width / num2, context.height / num2, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile8RT);
				cb.Blit(tile8RT, tileVRT, material, 3);
				cb.ReleaseTemporaryRT(tile8RT);
				int neighborMaxTex = MotionBlurComponent.Uniforms._NeighborMaxTex;
				int num4 = context.width / num2;
				int num5 = context.height / num2;
				cb.GetTemporaryRT(neighborMaxTex, num4, num5, 0, 0, this.m_VectorRTFormat, 1);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tileVRT);
				cb.Blit(tileVRT, neighborMaxTex, material, 4);
				cb.ReleaseTemporaryRT(tileVRT);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, (float)Mathf.Clamp(settings.sampleCount / 2, 1, 64));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, 5);
				cb.ReleaseTemporaryRT(velocityTex);
				cb.ReleaseTemporaryRT(neighborMaxTex);
			}

			// Token: 0x04000197 RID: 407
			private RenderTextureFormat m_VectorRTFormat = 13;

			// Token: 0x04000198 RID: 408
			private RenderTextureFormat m_PackedRTFormat = 8;
		}

		// Token: 0x02000060 RID: 96
		public class FrameBlendingFilter
		{
			// Token: 0x060001B8 RID: 440 RVA: 0x0000A016 File Offset: 0x00008216
			public FrameBlendingFilter()
			{
				this.m_UseCompression = MotionBlurComponent.FrameBlendingFilter.CheckSupportCompression();
				this.m_RawTextureFormat = MotionBlurComponent.FrameBlendingFilter.GetPreferredRenderTextureFormat();
				this.m_FrameList = new MotionBlurComponent.FrameBlendingFilter.Frame[4];
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x0000A040 File Offset: 0x00008240
			public void Dispose()
			{
				foreach (MotionBlurComponent.FrameBlendingFilter.Frame frame in this.m_FrameList)
				{
					frame.Release();
				}
			}

			// Token: 0x060001BA RID: 442 RVA: 0x0000A074 File Offset: 0x00008274
			public void PushFrame(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
			{
				int frameCount = Time.frameCount;
				if (frameCount == this.m_LastFrameCount)
				{
					return;
				}
				int num = frameCount % this.m_FrameList.Length;
				if (this.m_UseCompression)
				{
					this.m_FrameList[num].MakeRecord(cb, source, width, height, material);
				}
				else
				{
					this.m_FrameList[num].MakeRecordRaw(cb, source, width, height, this.m_RawTextureFormat);
				}
				this.m_LastFrameCount = frameCount;
			}

			// Token: 0x060001BB RID: 443 RVA: 0x0000A0E4 File Offset: 0x000082E4
			public void BlendFrames(CommandBuffer cb, float strength, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				float time = Time.time;
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative = this.GetFrameRelative(-1);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative2 = this.GetFrameRelative(-2);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative3 = this.GetFrameRelative(-3);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative4 = this.GetFrameRelative(-4);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frameRelative2.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frameRelative3.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frameRelative4.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frameRelative2.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frameRelative3.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frameRelative4.chromaTexture);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frameRelative2.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frameRelative3.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frameRelative4.CalculateWeight(strength, time));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, this.m_UseCompression ? 7 : 8);
			}

			// Token: 0x060001BC RID: 444 RVA: 0x0000A240 File Offset: 0x00008440
			private static bool CheckSupportCompression()
			{
				return SystemInfo.SupportsRenderTextureFormat(16) && SystemInfo.supportedRenderTargetCount > 1;
			}

			// Token: 0x060001BD RID: 445 RVA: 0x0000A258 File Offset: 0x00008458
			private static RenderTextureFormat GetPreferredRenderTextureFormat()
			{
				RenderTextureFormat[] array = new RenderTextureFormat[3];
				RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.04841F4DCEC5FD57BE2018FC808EA3A6F022D53A90A2CC7BE3B174D29A7D5D96).FieldHandle);
				foreach (RenderTextureFormat renderTextureFormat in array)
				{
					if (SystemInfo.SupportsRenderTextureFormat(renderTextureFormat))
					{
						return renderTextureFormat;
					}
				}
				return 7;
			}

			// Token: 0x060001BE RID: 446 RVA: 0x0000A294 File Offset: 0x00008494
			private MotionBlurComponent.FrameBlendingFilter.Frame GetFrameRelative(int offset)
			{
				int num = (Time.frameCount + this.m_FrameList.Length + offset) % this.m_FrameList.Length;
				return this.m_FrameList[num];
			}

			// Token: 0x04000199 RID: 409
			private bool m_UseCompression;

			// Token: 0x0400019A RID: 410
			private RenderTextureFormat m_RawTextureFormat;

			// Token: 0x0400019B RID: 411
			private MotionBlurComponent.FrameBlendingFilter.Frame[] m_FrameList;

			// Token: 0x0400019C RID: 412
			private int m_LastFrameCount;

			// Token: 0x02000094 RID: 148
			private struct Frame
			{
				// Token: 0x060001E3 RID: 483 RVA: 0x0000B2B0 File Offset: 0x000094B0
				public float CalculateWeight(float strength, float currentTime)
				{
					if (Mathf.Approximately(this.m_Time, 0f))
					{
						return 0f;
					}
					float num = Mathf.Lerp(80f, 16f, strength);
					return Mathf.Exp((this.m_Time - currentTime) * num);
				}

				// Token: 0x060001E4 RID: 484 RVA: 0x0000B2F8 File Offset: 0x000094F8
				public void Release()
				{
					if (this.lumaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.lumaTexture);
					}
					if (this.chromaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.chromaTexture);
					}
					this.lumaTexture = null;
					this.chromaTexture = null;
				}

				// Token: 0x060001E5 RID: 485 RVA: 0x0000B348 File Offset: 0x00009548
				public void MakeRecord(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, 16, 1);
					this.chromaTexture = RenderTexture.GetTemporary(width, height, 0, 16, 1);
					this.lumaTexture.filterMode = 0;
					this.chromaTexture.filterMode = 0;
					if (this.m_MRT == null)
					{
						this.m_MRT = new RenderTargetIdentifier[2];
					}
					this.m_MRT[0] = this.lumaTexture;
					this.m_MRT[1] = this.chromaTexture;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.SetRenderTarget(this.m_MRT, this.lumaTexture);
					cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 6);
					this.m_Time = Time.time;
				}

				// Token: 0x060001E6 RID: 486 RVA: 0x0000B41C File Offset: 0x0000961C
				public void MakeRecordRaw(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, RenderTextureFormat format)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format);
					this.lumaTexture.filterMode = 0;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.Blit(source, this.lumaTexture);
					this.m_Time = Time.time;
				}

				// Token: 0x04000293 RID: 659
				public RenderTexture lumaTexture;

				// Token: 0x04000294 RID: 660
				public RenderTexture chromaTexture;

				// Token: 0x04000295 RID: 661
				private float m_Time;

				// Token: 0x04000296 RID: 662
				private RenderTargetIdentifier[] m_MRT;
			}
		}
	}
}
