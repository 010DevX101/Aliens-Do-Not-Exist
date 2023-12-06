using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00007D10 File Offset: 0x00005F10
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00007D18 File Offset: 0x00005F18
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00007D27 File Offset: 0x00005F27
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00007D2F File Offset: 0x00005F2F
		public bool isDirty { get; internal set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00007D38 File Offset: 0x00005F38
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00007D40 File Offset: 0x00005F40
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x0600012B RID: 299 RVA: 0x00007D49 File Offset: 0x00005F49
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007D5C File Offset: 0x00005F5C
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x02000077 RID: 119
		public enum Tonemapper
		{
			// Token: 0x0400021D RID: 541
			None,
			// Token: 0x0400021E RID: 542
			ACES,
			// Token: 0x0400021F RID: 543
			Neutral
		}

		// Token: 0x02000078 RID: 120
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000AACC File Offset: 0x00008CCC
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x04000220 RID: 544
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x04000221 RID: 545
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x04000222 RID: 546
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x04000223 RID: 547
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x04000224 RID: 548
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x04000225 RID: 549
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x04000226 RID: 550
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02000079 RID: 121
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000AB34 File Offset: 0x00008D34
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x04000227 RID: 551
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04000228 RID: 552
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x04000229 RID: 553
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x0400022A RID: 554
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x0400022B RID: 555
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x0400022C RID: 556
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x0200007A RID: 122
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000AB94 File Offset: 0x00008D94
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x0400022D RID: 557
			public Vector3 red;

			// Token: 0x0400022E RID: 558
			public Vector3 green;

			// Token: 0x0400022F RID: 559
			public Vector3 blue;

			// Token: 0x04000230 RID: 560
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x0200007B RID: 123
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000AC04 File Offset: 0x00008E04
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x04000231 RID: 561
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x04000232 RID: 562
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x04000233 RID: 563
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x0200007C RID: 124
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x17000057 RID: 87
			// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000AC40 File Offset: 0x00008E40
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x04000234 RID: 564
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x04000235 RID: 565
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x04000236 RID: 566
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x0200007D RID: 125
		public enum ColorWheelMode
		{
			// Token: 0x04000238 RID: 568
			Linear,
			// Token: 0x04000239 RID: 569
			Log
		}

		// Token: 0x0200007E RID: 126
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000AC7C File Offset: 0x00008E7C
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400023A RID: 570
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x0400023B RID: 571
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x0400023C RID: 572
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x0200007F RID: 127
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000059 RID: 89
			// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000ACB4 File Offset: 0x00008EB4
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x0400023D RID: 573
			public ColorGradingCurve master;

			// Token: 0x0400023E RID: 574
			public ColorGradingCurve red;

			// Token: 0x0400023F RID: 575
			public ColorGradingCurve green;

			// Token: 0x04000240 RID: 576
			public ColorGradingCurve blue;

			// Token: 0x04000241 RID: 577
			public ColorGradingCurve hueVShue;

			// Token: 0x04000242 RID: 578
			public ColorGradingCurve hueVSsat;

			// Token: 0x04000243 RID: 579
			public ColorGradingCurve satVSsat;

			// Token: 0x04000244 RID: 580
			public ColorGradingCurve lumVSsat;

			// Token: 0x04000245 RID: 581
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04000246 RID: 582
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04000247 RID: 583
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04000248 RID: 584
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04000249 RID: 585
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x02000080 RID: 128
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005A RID: 90
			// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000AF3C File Offset: 0x0000913C
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400024A RID: 586
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x0400024B RID: 587
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x0400024C RID: 588
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x0400024D RID: 589
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x0400024E RID: 590
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
