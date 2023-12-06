using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000040 RID: 64
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("Effects/Post-Processing Behaviour", -1)]
	public class PostProcessingBehaviour : MonoBehaviour
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00007F34 File Offset: 0x00006134
		private void OnEnable()
		{
			this.m_CommandBuffers = new Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>>();
			this.m_MaterialFactory = new MaterialFactory();
			this.m_RenderTextureFactory = new RenderTextureFactory();
			this.m_Context = new PostProcessingContext();
			this.m_Components = new List<PostProcessingComponentBase>();
			this.m_DebugViews = this.AddComponent<BuiltinDebugViewsComponent>(new BuiltinDebugViewsComponent());
			this.m_AmbientOcclusion = this.AddComponent<AmbientOcclusionComponent>(new AmbientOcclusionComponent());
			this.m_ScreenSpaceReflection = this.AddComponent<ScreenSpaceReflectionComponent>(new ScreenSpaceReflectionComponent());
			this.m_FogComponent = this.AddComponent<FogComponent>(new FogComponent());
			this.m_MotionBlur = this.AddComponent<MotionBlurComponent>(new MotionBlurComponent());
			this.m_Taa = this.AddComponent<TaaComponent>(new TaaComponent());
			this.m_EyeAdaptation = this.AddComponent<EyeAdaptationComponent>(new EyeAdaptationComponent());
			this.m_DepthOfField = this.AddComponent<DepthOfFieldComponent>(new DepthOfFieldComponent());
			this.m_Bloom = this.AddComponent<BloomComponent>(new BloomComponent());
			this.m_ChromaticAberration = this.AddComponent<ChromaticAberrationComponent>(new ChromaticAberrationComponent());
			this.m_ColorGrading = this.AddComponent<ColorGradingComponent>(new ColorGradingComponent());
			this.m_UserLut = this.AddComponent<UserLutComponent>(new UserLutComponent());
			this.m_Grain = this.AddComponent<GrainComponent>(new GrainComponent());
			this.m_Vignette = this.AddComponent<VignetteComponent>(new VignetteComponent());
			this.m_Dithering = this.AddComponent<DitheringComponent>(new DitheringComponent());
			this.m_Fxaa = this.AddComponent<FxaaComponent>(new FxaaComponent());
			this.m_ComponentStates = new Dictionary<PostProcessingComponentBase, bool>();
			foreach (PostProcessingComponentBase key in this.m_Components)
			{
				this.m_ComponentStates.Add(key, false);
			}
			base.useGUILayout = false;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000080E8 File Offset: 0x000062E8
		private void OnPreCull()
		{
			this.m_Camera = base.GetComponent<Camera>();
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			PostProcessingContext postProcessingContext = this.m_Context.Reset();
			postProcessingContext.profile = this.profile;
			postProcessingContext.renderTextureFactory = this.m_RenderTextureFactory;
			postProcessingContext.materialFactory = this.m_MaterialFactory;
			postProcessingContext.camera = this.m_Camera;
			this.m_DebugViews.Init(postProcessingContext, this.profile.debugViews);
			this.m_AmbientOcclusion.Init(postProcessingContext, this.profile.ambientOcclusion);
			this.m_ScreenSpaceReflection.Init(postProcessingContext, this.profile.screenSpaceReflection);
			this.m_FogComponent.Init(postProcessingContext, this.profile.fog);
			this.m_MotionBlur.Init(postProcessingContext, this.profile.motionBlur);
			this.m_Taa.Init(postProcessingContext, this.profile.antialiasing);
			this.m_EyeAdaptation.Init(postProcessingContext, this.profile.eyeAdaptation);
			this.m_DepthOfField.Init(postProcessingContext, this.profile.depthOfField);
			this.m_Bloom.Init(postProcessingContext, this.profile.bloom);
			this.m_ChromaticAberration.Init(postProcessingContext, this.profile.chromaticAberration);
			this.m_ColorGrading.Init(postProcessingContext, this.profile.colorGrading);
			this.m_UserLut.Init(postProcessingContext, this.profile.userLut);
			this.m_Grain.Init(postProcessingContext, this.profile.grain);
			this.m_Vignette.Init(postProcessingContext, this.profile.vignette);
			this.m_Dithering.Init(postProcessingContext, this.profile.dithering);
			this.m_Fxaa.Init(postProcessingContext, this.profile.antialiasing);
			if (this.m_PreviousProfile != this.profile)
			{
				this.DisableComponents();
				this.m_PreviousProfile = this.profile;
			}
			this.CheckObservers();
			DepthTextureMode depthTextureMode = postProcessingContext.camera.depthTextureMode;
			foreach (PostProcessingComponentBase postProcessingComponentBase in this.m_Components)
			{
				if (postProcessingComponentBase.active)
				{
					depthTextureMode |= postProcessingComponentBase.GetCameraFlags();
				}
			}
			postProcessingContext.camera.depthTextureMode = depthTextureMode;
			if (!this.m_RenderingInSceneView && this.m_Taa.active && !this.profile.debugViews.willInterrupt)
			{
				this.m_Taa.SetProjectionMatrix(this.jitteredMatrixFunc);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008398 File Offset: 0x00006598
		private void OnPreRender()
		{
			if (this.profile == null)
			{
				return;
			}
			this.TryExecuteCommandBuffer<BuiltinDebugViewsModel>(this.m_DebugViews);
			this.TryExecuteCommandBuffer<AmbientOcclusionModel>(this.m_AmbientOcclusion);
			this.TryExecuteCommandBuffer<ScreenSpaceReflectionModel>(this.m_ScreenSpaceReflection);
			this.TryExecuteCommandBuffer<FogModel>(this.m_FogComponent);
			if (!this.m_RenderingInSceneView)
			{
				this.TryExecuteCommandBuffer<MotionBlurModel>(this.m_MotionBlur);
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000083F8 File Offset: 0x000065F8
		private void OnPostRender()
		{
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			if (!this.m_RenderingInSceneView && this.m_Taa.active && !this.profile.debugViews.willInterrupt)
			{
				this.m_Context.camera.ResetProjectionMatrix();
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000845C File Offset: 0x0000665C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.profile == null || this.m_Camera == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			bool flag = false;
			bool active = this.m_Fxaa.active;
			bool flag2 = this.m_Taa.active && !this.m_RenderingInSceneView;
			bool flag3 = this.m_DepthOfField.active && !this.m_RenderingInSceneView;
			Material material = this.m_MaterialFactory.Get("Hidden/Post FX/Uber Shader");
			material.shaderKeywords = null;
			RenderTexture renderTexture = source;
			if (flag2)
			{
				RenderTexture renderTexture2 = this.m_RenderTextureFactory.Get(renderTexture);
				this.m_Taa.Render(renderTexture, renderTexture2);
				renderTexture = renderTexture2;
			}
			Texture texture = GraphicsUtils.whiteTexture;
			if (this.m_EyeAdaptation.active)
			{
				flag = true;
				texture = this.m_EyeAdaptation.Prepare(renderTexture, material);
			}
			material.SetTexture("_AutoExposure", texture);
			if (flag3)
			{
				flag = true;
				this.m_DepthOfField.Prepare(renderTexture, material, flag2, this.m_Taa.jitterVector, this.m_Taa.model.settings.taaSettings.motionBlending);
			}
			if (this.m_Bloom.active)
			{
				flag = true;
				this.m_Bloom.Prepare(renderTexture, material, texture);
			}
			flag |= this.TryPrepareUberImageEffect<ChromaticAberrationModel>(this.m_ChromaticAberration, material);
			flag |= this.TryPrepareUberImageEffect<ColorGradingModel>(this.m_ColorGrading, material);
			flag |= this.TryPrepareUberImageEffect<VignetteModel>(this.m_Vignette, material);
			flag |= this.TryPrepareUberImageEffect<UserLutModel>(this.m_UserLut, material);
			Material material2 = active ? this.m_MaterialFactory.Get("Hidden/Post FX/FXAA") : null;
			if (active)
			{
				material2.shaderKeywords = null;
				this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material2);
				this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material2);
				if (flag)
				{
					RenderTexture renderTexture3 = this.m_RenderTextureFactory.Get(renderTexture);
					Graphics.Blit(renderTexture, renderTexture3, material, 0);
					renderTexture = renderTexture3;
				}
				this.m_Fxaa.Render(renderTexture, destination);
			}
			else
			{
				flag |= this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material);
				flag |= this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material);
				if (flag)
				{
					if (!GraphicsUtils.isLinearColorSpace)
					{
						material.EnableKeyword("UNITY_COLORSPACE_GAMMA");
					}
					Graphics.Blit(renderTexture, destination, material, 0);
				}
			}
			if (!flag && !active)
			{
				Graphics.Blit(renderTexture, destination);
			}
			this.m_RenderTextureFactory.ReleaseAll();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000086A8 File Offset: 0x000068A8
		private void OnGUI()
		{
			if (Event.current.type != 7)
			{
				return;
			}
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			if (this.m_EyeAdaptation.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				this.m_EyeAdaptation.OnGUI();
				return;
			}
			if (this.m_ColorGrading.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut))
			{
				this.m_ColorGrading.OnGUI();
				return;
			}
			if (this.m_UserLut.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut))
			{
				this.m_UserLut.OnGUI();
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008764 File Offset: 0x00006964
		private void OnDisable()
		{
			foreach (KeyValuePair<CameraEvent, CommandBuffer> keyValuePair in this.m_CommandBuffers.Values)
			{
				this.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
				keyValuePair.Value.Dispose();
			}
			this.m_CommandBuffers.Clear();
			if (this.profile != null)
			{
				this.DisableComponents();
			}
			this.m_Components.Clear();
			this.m_MaterialFactory.Dispose();
			this.m_RenderTextureFactory.Dispose();
			GraphicsUtils.Dispose();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008820 File Offset: 0x00006A20
		public void ResetTemporalEffects()
		{
			this.m_Taa.ResetHistory();
			this.m_MotionBlur.ResetHistory();
			this.m_EyeAdaptation.ResetHistory();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008844 File Offset: 0x00006A44
		private void CheckObservers()
		{
			foreach (KeyValuePair<PostProcessingComponentBase, bool> keyValuePair in this.m_ComponentStates)
			{
				PostProcessingComponentBase key = keyValuePair.Key;
				bool enabled = key.GetModel().enabled;
				if (enabled != keyValuePair.Value)
				{
					if (enabled)
					{
						this.m_ComponentsToEnable.Add(key);
					}
					else
					{
						this.m_ComponentsToDisable.Add(key);
					}
				}
			}
			for (int i = 0; i < this.m_ComponentsToDisable.Count; i++)
			{
				PostProcessingComponentBase postProcessingComponentBase = this.m_ComponentsToDisable[i];
				this.m_ComponentStates[postProcessingComponentBase] = false;
				postProcessingComponentBase.OnDisable();
			}
			for (int j = 0; j < this.m_ComponentsToEnable.Count; j++)
			{
				PostProcessingComponentBase postProcessingComponentBase2 = this.m_ComponentsToEnable[j];
				this.m_ComponentStates[postProcessingComponentBase2] = true;
				postProcessingComponentBase2.OnEnable();
			}
			this.m_ComponentsToDisable.Clear();
			this.m_ComponentsToEnable.Clear();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00008960 File Offset: 0x00006B60
		private void DisableComponents()
		{
			foreach (PostProcessingComponentBase postProcessingComponentBase in this.m_Components)
			{
				PostProcessingModel model = postProcessingComponentBase.GetModel();
				if (model != null && model.enabled)
				{
					postProcessingComponentBase.OnDisable();
				}
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000089C4 File Offset: 0x00006BC4
		private CommandBuffer AddCommandBuffer<T>(CameraEvent evt, string name) where T : PostProcessingModel
		{
			CommandBuffer value = new CommandBuffer
			{
				name = name
			};
			KeyValuePair<CameraEvent, CommandBuffer> value2 = new KeyValuePair<CameraEvent, CommandBuffer>(evt, value);
			this.m_CommandBuffers.Add(typeof(!!0), value2);
			this.m_Camera.AddCommandBuffer(evt, value2.Value);
			return value2.Value;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008A18 File Offset: 0x00006C18
		private void RemoveCommandBuffer<T>() where T : PostProcessingModel
		{
			Type typeFromHandle = typeof(!!0);
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair;
			if (!this.m_CommandBuffers.TryGetValue(typeFromHandle, out keyValuePair))
			{
				return;
			}
			this.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
			this.m_CommandBuffers.Remove(typeFromHandle);
			keyValuePair.Value.Dispose();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008A74 File Offset: 0x00006C74
		private CommandBuffer GetCommandBuffer<T>(CameraEvent evt, string name) where T : PostProcessingModel
		{
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair;
			CommandBuffer result;
			if (!this.m_CommandBuffers.TryGetValue(typeof(!!0), out keyValuePair))
			{
				result = this.AddCommandBuffer<T>(evt, name);
			}
			else if (keyValuePair.Key != evt)
			{
				this.RemoveCommandBuffer<T>();
				result = this.AddCommandBuffer<T>(evt, name);
			}
			else
			{
				result = keyValuePair.Value;
			}
			return result;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008ACC File Offset: 0x00006CCC
		private void TryExecuteCommandBuffer<T>(PostProcessingComponentCommandBuffer<T> component) where T : PostProcessingModel
		{
			if (component.active)
			{
				CommandBuffer commandBuffer = this.GetCommandBuffer<T>(component.GetCameraEvent(), component.GetName());
				commandBuffer.Clear();
				component.PopulateCommandBuffer(commandBuffer);
				return;
			}
			this.RemoveCommandBuffer<T>();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008B08 File Offset: 0x00006D08
		private bool TryPrepareUberImageEffect<T>(PostProcessingComponentRenderTexture<T> component, Material material) where T : PostProcessingModel
		{
			if (!component.active)
			{
				return false;
			}
			component.Prepare(material);
			return true;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008B1C File Offset: 0x00006D1C
		private T AddComponent<T>(T component) where T : PostProcessingComponentBase
		{
			this.m_Components.Add(component);
			return component;
		}

		// Token: 0x040000D4 RID: 212
		public PostProcessingProfile profile;

		// Token: 0x040000D5 RID: 213
		public Func<Vector2, Matrix4x4> jitteredMatrixFunc;

		// Token: 0x040000D6 RID: 214
		private Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>> m_CommandBuffers;

		// Token: 0x040000D7 RID: 215
		private List<PostProcessingComponentBase> m_Components;

		// Token: 0x040000D8 RID: 216
		private Dictionary<PostProcessingComponentBase, bool> m_ComponentStates;

		// Token: 0x040000D9 RID: 217
		private MaterialFactory m_MaterialFactory;

		// Token: 0x040000DA RID: 218
		private RenderTextureFactory m_RenderTextureFactory;

		// Token: 0x040000DB RID: 219
		private PostProcessingContext m_Context;

		// Token: 0x040000DC RID: 220
		private Camera m_Camera;

		// Token: 0x040000DD RID: 221
		private PostProcessingProfile m_PreviousProfile;

		// Token: 0x040000DE RID: 222
		private bool m_RenderingInSceneView;

		// Token: 0x040000DF RID: 223
		private BuiltinDebugViewsComponent m_DebugViews;

		// Token: 0x040000E0 RID: 224
		private AmbientOcclusionComponent m_AmbientOcclusion;

		// Token: 0x040000E1 RID: 225
		private ScreenSpaceReflectionComponent m_ScreenSpaceReflection;

		// Token: 0x040000E2 RID: 226
		private FogComponent m_FogComponent;

		// Token: 0x040000E3 RID: 227
		private MotionBlurComponent m_MotionBlur;

		// Token: 0x040000E4 RID: 228
		private TaaComponent m_Taa;

		// Token: 0x040000E5 RID: 229
		private EyeAdaptationComponent m_EyeAdaptation;

		// Token: 0x040000E6 RID: 230
		private DepthOfFieldComponent m_DepthOfField;

		// Token: 0x040000E7 RID: 231
		private BloomComponent m_Bloom;

		// Token: 0x040000E8 RID: 232
		private ChromaticAberrationComponent m_ChromaticAberration;

		// Token: 0x040000E9 RID: 233
		private ColorGradingComponent m_ColorGrading;

		// Token: 0x040000EA RID: 234
		private UserLutComponent m_UserLut;

		// Token: 0x040000EB RID: 235
		private GrainComponent m_Grain;

		// Token: 0x040000EC RID: 236
		private VignetteComponent m_Vignette;

		// Token: 0x040000ED RID: 237
		private DitheringComponent m_Dithering;

		// Token: 0x040000EE RID: 238
		private FxaaComponent m_Fxaa;

		// Token: 0x040000EF RID: 239
		private List<PostProcessingComponentBase> m_ComponentsToEnable = new List<PostProcessingComponentBase>();

		// Token: 0x040000F0 RID: 240
		private List<PostProcessingComponentBase> m_ComponentsToDisable = new List<PostProcessingComponentBase>();
	}
}
