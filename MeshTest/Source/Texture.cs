using System;
using System.Drawing;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace MeshTest
{
	public class Texture
	{
		int _glHandle;
		public int Handle
		{
			get { return _glHandle; }
		}
		public Texture (string filename)
		{
			if (!System.IO.File.Exists(Paths.TexturePath + filename))
				throw new FileNotFoundException("image: " + filename + " not found in " + Paths.TexturePath);
			Bitmap textureBitmap = new Bitmap(Paths.TexturePath + filename);
			
			System.Drawing.Imaging.BitmapData textureData = textureBitmap.LockBits(new Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
			                                                                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
			                                                                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.GenTextures(1, out _glHandle);
			GL.BindTexture(TextureTarget.Texture2D, _glHandle);			
			
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, textureBitmap.Width, textureBitmap.Height, 0, 
			              PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			textureBitmap.UnlockBits(textureData);
		}
	}
}

