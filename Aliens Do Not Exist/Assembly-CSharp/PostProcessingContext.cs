using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000045 RID: 69
	public class PostProcessingContext
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00008BA5 File Offset: 0x00006DA5
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00008BAD File Offset: 0x00006DAD
		public bool interrupted { get; private set; }

		// Token: 0x06000176 RID: 374 RVA: 0x00008BB6 File Offset: 0x00006DB6
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008BBF File Offset: 0x00006DBF
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00008BE5 File Offset: 0x00006DE5
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == 3;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00008BF5 File Offset: 0x00006DF5
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00008C02 File Offset: 0x00006E02
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00008C0F File Offset: 0x00006E0F
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00008C1C File Offset: 0x00006E1C
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x040000F3 RID: 243
		public PostProcessingProfile profile;

		// Token: 0x040000F4 RID: 244
		public Camera camera;

		// Token: 0x040000F5 RID: 245
		public MaterialFactory materialFactory;

		// Token: 0x040000F6 RID: 246
		public RenderTextureFactory renderTextureFactory;
	}
}
