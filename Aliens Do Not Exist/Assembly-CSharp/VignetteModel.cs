using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007F00 File Offset: 0x00006100
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00007F08 File Offset: 0x00006108
		public VignetteModel.Settings settings
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

		// Token: 0x06000150 RID: 336 RVA: 0x00007F11 File Offset: 0x00006111
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x040000D3 RID: 211
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x02000090 RID: 144
		public enum Mode
		{
			// Token: 0x04000288 RID: 648
			Classic,
			// Token: 0x04000289 RID: 649
			Masked
		}

		// Token: 0x02000091 RID: 145
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000063 RID: 99
			// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000B21C File Offset: 0x0000941C
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x0400028A RID: 650
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x0400028B RID: 651
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x0400028C RID: 652
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x0400028D RID: 653
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x0400028E RID: 654
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x0400028F RID: 655
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04000290 RID: 656
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04000291 RID: 657
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04000292 RID: 658
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
