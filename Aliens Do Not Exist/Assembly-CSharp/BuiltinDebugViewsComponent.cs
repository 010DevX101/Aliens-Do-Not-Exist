using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000023 RID: 35
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004518 File Offset: 0x00002718
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004544 File Offset: 0x00002744
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = 0;
			switch (mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				depthTextureMode |= 1;
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				depthTextureMode |= 2;
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				depthTextureMode |= 5;
				break;
			}
			return depthTextureMode;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000458B File Offset: 0x0000278B
		public override CameraEvent GetCameraEvent()
		{
			if (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors)
			{
				return 12;
			}
			return 18;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000045A5 File Offset: 0x000027A5
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000045AC File Offset: 0x000027AC
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ref BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			switch (settings.mode)
			{
			case BuiltinDebugViewsModel.Mode.Depth:
				this.DepthPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.Normals:
				this.DepthNormalsPass(cb);
				break;
			case BuiltinDebugViewsModel.Mode.MotionVectors:
				this.MotionVectorsPass(cb);
				break;
			}
			this.context.Interrupt();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000463C File Offset: 0x0000283C
		private void DepthPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, 2, material, 0);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004698 File Offset: 0x00002898
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, 2, material, 1);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000046CC File Offset: 0x000028CC
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int num = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(num, this.context.width, this.context.height, 0, 1);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, 2);
			cb.Blit(2, num, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, 1);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
				cb.Blit(num, tempRT, material, 3);
				cb.ReleaseTemporaryRT(num);
				num = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num2 = 1f / (float)motionVectors.motionVectorsResolution;
				float num3 = num2 * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(num3, num2));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
			cb.Blit(num, 2);
			cb.ReleaseTemporaryRT(num);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000048C0 File Offset: 0x00002AC0
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004938 File Offset: 0x00002B38
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x04000099 RID: 153
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x0400009A RID: 154
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x02000052 RID: 82
		private static class Uniforms
		{
			// Token: 0x0400012F RID: 303
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x04000130 RID: 304
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04000131 RID: 305
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x04000132 RID: 306
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000133 RID: 307
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x04000134 RID: 308
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x04000135 RID: 309
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x02000053 RID: 83
		private enum Pass
		{
			// Token: 0x04000137 RID: 311
			Depth,
			// Token: 0x04000138 RID: 312
			Normals,
			// Token: 0x04000139 RID: 313
			MovecOpacity,
			// Token: 0x0400013A RID: 314
			MovecImaging,
			// Token: 0x0400013B RID: 315
			MovecArrows
		}

		// Token: 0x02000054 RID: 84
		private class ArrowArray
		{
			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000961E File Offset: 0x0000781E
			// (set) Token: 0x060001A3 RID: 419 RVA: 0x00009626 File Offset: 0x00007826
			public Mesh mesh { get; private set; }

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000962F File Offset: 0x0000782F
			// (set) Token: 0x060001A5 RID: 421 RVA: 0x00009637 File Offset: 0x00007837
			public int columnCount { get; private set; }

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x060001A6 RID: 422 RVA: 0x00009640 File Offset: 0x00007840
			// (set) Token: 0x060001A7 RID: 423 RVA: 0x00009648 File Offset: 0x00007848
			public int rowCount { get; private set; }

			// Token: 0x060001A8 RID: 424 RVA: 0x00009654 File Offset: 0x00007854
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item;
						item..ctor((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = 52
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, 3, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x000097F7 File Offset: 0x000079F7
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
