using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000047 RID: 71
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x040000F9 RID: 249
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x040000FA RID: 250
		public FogModel fog = new FogModel();

		// Token: 0x040000FB RID: 251
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x040000FC RID: 252
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x040000FD RID: 253
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x040000FE RID: 254
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x040000FF RID: 255
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04000100 RID: 256
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04000101 RID: 257
		public BloomModel bloom = new BloomModel();

		// Token: 0x04000102 RID: 258
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04000103 RID: 259
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04000104 RID: 260
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04000105 RID: 261
		public GrainModel grain = new GrainModel();

		// Token: 0x04000106 RID: 262
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x04000107 RID: 263
		public DitheringModel dithering = new DitheringModel();
	}
}
