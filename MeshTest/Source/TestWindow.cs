//ASCII Contents of Fragment 3303426 in sda2-0-0


using System;
using System.Xml;
using System.Xml.Schema;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ModelMesh;
using ColladaImporter;

namespace MeshTest
{	
	public class TestWindow : GameWindow
	{
		int vbo;
		int debugVAO;
		Shader shader;
		Shader lineDrawer;
		int shaderProgram;
		int wvp;
		double counter = 0;
		Matrix4 WVP;
		Vector3[] vertices;
		MeshElement mesh;
		Vector3 cameraPosition;
		Vector2 cameraOrientation;
		Matrix4 world, view, projection; 
		int mouseX = 400;
		int mouseY = 300;
		MouseState lastState;
		Vector2 lastDelta = Vector2.Zero;
		const float mouseMultiplier = 2.0f;
		Texture cubetex;
		Texture cubenorm;
		
		public TestWindow () : 
			base (640, 480, OpenTK.Graphics.GraphicsMode.Default, "test", GameWindowFlags.Default)
		{
			
			GL.ClearColor(OpenTK.Graphics.Color4.Wheat);
			Keyboard.KeyDown += HandleKeyboardKeyDown;
			WVP = Matrix4.Identity;
			cameraPosition = Vector3.UnitZ * - 10.0f;
			cameraOrientation = Vector2.Zero;
			projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 4.0f / 3.0f, 0.01f, 100.0f);
			lastState = new MouseState();
			Paths.Init();
		}

			void HandleKeyboardKeyDown (object sender, OpenTK.Input.KeyboardKeyEventArgs e)
			{
				if (e.Key == OpenTK.Input.Key.Escape)
					Exit();
			}	
		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			world = Matrix4.CreateRotationX(MathHelper.RadiansToDegrees(90));
			Vector3 forwardVec = Vector3.Transform(Vector3.UnitZ, Matrix4.CreateRotationY(cameraOrientation.Y));
			Vector3 rightVec = Vector3.Transform(Vector3.UnitZ, Matrix4.CreateRotationY(cameraOrientation.Y - MathHelper.DegreesToRadians(90.0f)));
			
			Vector3 camTarget = cameraPosition + Vector3.Transform(Vector3.UnitZ, Matrix4.CreateRotationX(cameraOrientation.X) * Matrix4.CreateRotationY(cameraOrientation.Y));
			view = Matrix4.LookAt(cameraPosition, camTarget, Vector3.UnitY);
			
			if (Keyboard[Key.W])
			{
				cameraPosition += forwardVec * 0.2f;
			}
			else if (Keyboard[Key.S])
			{
				cameraPosition -= forwardVec * 0.2f;
			}
			
			if (Keyboard[Key.D])
			{
				cameraPosition += rightVec * 0.2f;
			}
			else if (Keyboard[Key.A])
			{
				cameraPosition -= rightVec * 0.2f;
			}
			
			if (Keyboard[Key.Space])
			{
				cameraPosition.Y += 0.2f;
			}
			else if (Keyboard[Key.ControlLeft])
			{
				cameraPosition.Y -= 0.2f;
			}
			
			if (Keyboard[Key.Up])
				cameraOrientation.X -= 0.02f;
			else if (Keyboard[Key.Down])
				cameraOrientation.X += 0.02f;
			
			if (Keyboard[Key.Left])
				cameraOrientation.Y += 0.02f;
			else if (Keyboard[Key.Right])
				cameraOrientation.Y -= 0.02f;
			
			var state = OpenTK.Input.Mouse.GetState();
			
			Vector2 mouseDelta = new Vector2((float)(state.Y - lastState.Y), (float)(lastState.X - state.X));			
			
			if (mouseDelta.Length > 0.001f)
			{
				mouseDelta.Normalize();				
			}

			Easing.Logarithmic(ref lastDelta.X, mouseDelta.X, 1.0f);
			Easing.Logarithmic(ref lastDelta.Y, mouseDelta.Y, 1.0f);			                   
			
			/*			
			if (mouseDelta.X > lastDelta.X + 0.02f)
			{
				lastDelta.X += 0.02f;
			}
			else if (mouseDelta.X < lastDelta.X - 0.03f)
			{
				lastDelta.X -= 0.03f;
			}
			else
			{
				lastDelta.X = mouseDelta.X;
			}
			
			if (mouseDelta.Y > lastDelta.Y + 0.02f)
			{
				lastDelta.Y += 0.02f;
			}
			else if (mouseDelta.Y < lastDelta.Y - 0.03f)
			{
				lastDelta.Y -= 0.03f;
			}
			else
			{
				lastDelta.Y = mouseDelta.Y;
			}
			*/
			
			cameraOrientation += lastDelta * (float)e.Time * mouseMultiplier;
			
			lastState = state;
			
			//OpenTK.Input.Mouse.SetPosition((double)mouseX, (double)mouseY);				

			WVP = world * view * projection;
			
			base.OnUpdateFrame (e);
		}
		
		
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);			
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.UseProgram(shader.Handle);
			GL.UniformMatrix4(shader.uniforms["WVP"], false, ref WVP);
			//GL.UniformMatrix4(shader.uniforms["InverseProj"], false, ref InverseProj);
			counter += 0.01f;
			Vector3 lightpos = new Vector3((float)Math.Sin(counter), (float)Math.Cos(counter), 0.0f);
			lightpos *= 10.0f;
			GL.Uniform3(shader.uniforms["LightPos"], lightpos);
			GL.Uniform3(shader.uniforms["LightColor"], new Vector3(1.0f, 0.75f, 0.75f));
			GL.Uniform3(shader.uniforms["camPos"], cameraPosition);
			
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, cubetex.Handle);
			GL.Uniform1(shader.uniforms["diffuseMap"], 0);
			
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, cubenorm.Handle);
			GL.Uniform1(shader.uniforms["normalMap"], 1);
			
			/*
			GL.EnableVertexAttribArray(0);	
			GL.EnableVertexAttribArray(1);
			
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, buf);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, buf2);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, mesh.VertexDeclaration.Stride * sizeof(float), 0);			
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, mesh.VertexDeclaration.Stride * sizeof(float), 12);
			GL.DrawElements(BeginMode.Triangles, mesh.IndexBuffer.Length, DrawElementsType.UnsignedShort, 0);
			
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);			*/
			
			mesh.Draw();
			
			/*GL.UseProgram(lineDrawer.Handle);
			GL.UniformMatrix4(lineDrawer.uniforms["WVP"], false, ref WVP);
			GL.BindVertexArray(debugVAO);
			GL.DrawArrays(BeginMode.Lines, 0, mesh.VertexCount * 2);*/
			
			SwapBuffers();
			
			base.OnRenderFrame (e);
		}
		protected override void OnLoad (EventArgs e)
		{
			
			ColladaXML daeReader = new ColladaXML("collada_schema_1_4.xsd");
			Console.WriteLine("Parsing File...");
			daeReader.Parse(Paths.ModelPath + "pixtest.dae");
			mesh = daeReader.Mesh.Elements[0];
			mesh.Optimise(new NormalSmoother());
			mesh.CreateGPUBuffers();
			GL.ClearColor(OpenTK.Graphics.Color4.Wheat);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.CullFace(CullFaceMode.Back);
			shader = new Shader("hello-gl.v.glsl", "hello-gl.f.glsl");
			lineDrawer = new Shader("linedrawer.v.glsl", "linedrawer.f.glsl");
			
			/*
			GL.GenBuffers(1, out buf);
			GL.BindBuffer(BufferTarget.ArrayBuffer, buf);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(mesh.VertexBuffer.Length * sizeof(float)), mesh.VertexBuffer, BufferUsageHint.StaticDraw);
			GL.GenBuffers(2, out buf2);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, buf2);
			GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(mesh.IndexBuffer.Length * sizeof(ushort)), mesh.IndexBuffer, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			*/
			CreateShaders();	
			mouseX = X + (Width / 2);
			mouseY = Y + (Height / 2);
			
			CursorVisible = false;
			
			OpenTK.Input.Mouse.SetPosition((double)mouseX, (double)mouseY);
			lastState = OpenTK.Input.Mouse.GetState();
			
			CursorVisible = false;
			GenerateDebugBuffer("NORMAL");
			cubetex = new Texture("pixdiffuse.png");
			cubenorm = new Texture("pixtestnorm.png");
			base.OnLoad (e);
		}
		void GenerateDebugBuffer(string debugType)
		{			
			float[] data = new float[mesh.VertexCount * 6];
			for (int i = 0; i < mesh.VertexCount; ++i)
			{
				mesh.ReadAttribute("VERTEX", i).CopyTo(data, 0, i * 6);
				Vector3 vec = VectorMethods.FromArray3(mesh.ReadAttribute(debugType, i)) + VectorMethods.FromArray3(mesh.ReadAttribute("VERTEX", i));
				vec.ToArray().CopyTo(data, 0, (i * 6) + 3);
			}
			GL.GenVertexArrays(1, out debugVAO);
			int buf;
			GL.GenBuffers(1, out buf);
			GL.BindVertexArray(debugVAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, buf);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.BindVertexArray(0);
			
		}
		void CreateBuffer()
		{
			vertices = new Vector3[3];
			vertices[0] = new Vector3(-10.0f, -10.0f, 0.0f);
			vertices[1] = new Vector3(10.0f, -10.0f, 0.0f);
			vertices[2] = new Vector3(0.0f, 10.0f, 0.0f);
			
			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
		}
		void CreateShaders()
		{
			string vertexSource = @"#version 330

layout(location = 0) in vec3 Position;

uniform mat4 WVP;

void main()
{	
    gl_Position = WVP * vec4(Position, 1.0);
}";
				
			string fragmentSource = @"#version 330

out vec4 frag_Color;

void main()
{
    frag_Color = vec4(0.0, 0.0, 1.0, 1.0);
}";
			int vertexShader, fragmentShader;
			
			vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexSource);
			GL.CompileShader(vertexShader);
			
			fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentSource);
			GL.CompileShader(fragmentShader);
			
			shaderProgram = GL.CreateProgram();
			GL.AttachShader(shaderProgram, vertexShader);
			GL.AttachShader(shaderProgram, fragmentShader);
			GL.LinkProgram(shaderProgram);
			
			Console.WriteLine(GL.GetProgramInfoLog(shaderProgram));
			
			GL.UseProgram(shaderProgram);	
			
			wvp = GL.GetUniformLocation(shaderProgram, "WVP");
			if (wvp < 0)
				throw new Exception("could not get uniform location");
			
		}
		public static void Main()
		{
			var window = new TestWindow();
			window.Run(60);
		}
		
	}
}



