using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00008D10 File Offset: 0x00006F10
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008D3C File Offset: 0x00006F3C
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe keyframe = this.curve[length - 1];
			keyframe.time -= this.m_Range;
			Keyframe keyframe2 = this.curve[0];
			keyframe2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(keyframe);
			this.m_InternalLoopingCurve.AddKey(keyframe2);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008DEC File Offset: 0x00006FEC
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x04000108 RID: 264
		public AnimationCurve curve;

		// Token: 0x04000109 RID: 265
		[SerializeField]
		private bool m_Loop;

		// Token: 0x0400010A RID: 266
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x0400010B RID: 267
		[SerializeField]
		private float m_Range;

		// Token: 0x0400010C RID: 268
		private AnimationCurve m_InternalLoopingCurve;
	}
}
