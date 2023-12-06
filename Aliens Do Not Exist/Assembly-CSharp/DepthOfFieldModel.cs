using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00007D78 File Offset: 0x00005F78
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00007D80 File Offset: 0x00005F80
		public DepthOfFieldModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007D89 File Offset: 0x00005F89
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x02000081 RID: 129
		public enum KernelSize
		{
			// Token: 0x04000250 RID: 592
			Small,
			// Token: 0x04000251 RID: 593
			Medium,
			// Token: 0x04000252 RID: 594
			Large,
			// Token: 0x04000253 RID: 595
			VeryLarge
		}

		// Token: 0x02000082 RID: 130
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005B RID: 91
			// (get) Token: 0x060001DA RID: 474 RVA: 0x0000AF90 File Offset: 0x00009190
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x04000254 RID: 596
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04000255 RID: 597
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04000256 RID: 598
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04000257 RID: 599
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04000258 RID: 600
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
