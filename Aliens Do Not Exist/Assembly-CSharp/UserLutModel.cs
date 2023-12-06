using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00007ECF File Offset: 0x000060CF
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00007ED7 File Offset: 0x000060D7
		public UserLutModel.Settings settings
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

		// Token: 0x0600014C RID: 332 RVA: 0x00007EE0 File Offset: 0x000060E0
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x040000D2 RID: 210
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x0200008F RID: 143
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000B1F0 File Offset: 0x000093F0
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04000285 RID: 645
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04000286 RID: 646
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
