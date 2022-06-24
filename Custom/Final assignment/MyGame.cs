using System;
using System.Collections.Generic;
using System.Drawing;

namespace GXPEngine;

public class MyGame : Game
{
	private bool stepped = false;
	private bool paused = false;
	public bool bulletCollide = false;
	private int stepIndex = 0;

	readonly Canvas lineContainer = null;

	public readonly List<Ball> movers;
	public readonly List<Triangle> triangles;




	public int GetNumberOfMovers()
	{
		return movers.Count;
	}

	public Ball GetMover(int index)
	{
		if (index >= 0 && index < movers.Count)
		{
			return movers[index];
		}

		return null;
	}

	public void DrawLine(Vec2 start, Vec2 end)
	{
		lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
	}

	private MyGame() : base(1200, 900, false, false)
	{
		lineContainer = new Canvas(width, height);
		AddChild(lineContainer);

		targetFps = 60;

		movers = new List<Ball>();
		triangles = new List<Triangle>();


		PrintInfo();

		LoadLevel(0);
	}

	private void LoadLevel(int index)
	{
		foreach (GameObject g in GetChildren())
		{
			g.LateDestroy();
		}
		foreach (Ball mover in movers)
		{
			mover.LateDestroy();
		}

		movers.Clear();

		foreach (Triangle t in triangles)
		{
			t.LateDestroy();
		}

		triangles.Clear();

		ArrayLevel level = new (index);
		AddChild(level);
	}
	/****************************************************************************************/

	void PrintInfo()
	{
		Console.WriteLine("Hold spacebar to slow down the frame rate.");
		Console.WriteLine("Use arrow keys and backspace to set the gravity.");
		Console.WriteLine("Press S to toggle stepped mode.");
		Console.WriteLine("Press P to toggle pause.");
		Console.WriteLine("Press D to draw debug lines.");
		Console.WriteLine("Press C to clear all debug lines.");
		Console.WriteLine("Press R to reset scene, and numbers to load different scenes.");
		Console.WriteLine("Press B to toggle high/low bounciness.");
		Console.WriteLine("Press W to toggle extra output text.");
	}

	private void HandleInput()
	{
		targetFps = Input.GetKey(Key.SPACE) ? 5 : 60;
		if (Input.GetKeyDown(Key.UP))
		{
			Ball.acceleration.SetXy(0, -1);
		}

		if (Input.GetKeyDown(Key.S))
		{
			stepped ^= true;
		}

		if (Input.GetKeyDown(Key.D))
		{
			Ball.drawDebugLine ^= true;
		}

		if (Input.GetKeyDown(Key.P))
		{
			paused ^= true;
		}

		if (Input.GetKeyDown(Key.R))
		{
			bulletCollide = !bulletCollide;
		}

		if (Input.GetKeyDown(Key.B))
		{
			Ball.bounciness = 1.5f - Ball.bounciness;
		}

		if (Input.GetKeyDown(Key.W))
		{
			LoadLevel(0);
		}

		if (Input.GetKeyDown(Key.C))
		{
			lineContainer.graphics.Clear(Color.Black);
		}
	}

	private void StepThroughMovers()
	{
		if (stepped)
		{
			// move everything step-by-step: in one frame, only one mover moves
			stepIndex++;
			if (stepIndex >= movers.Count)
			{
				stepIndex = 0;
			}

			if (movers[stepIndex].moving)
			{
				movers[stepIndex].Step();
			}
		}
		else
		{
			// move all movers every frame
			foreach (Ball mover in movers)
			{
				if (mover.moving)
				{
					mover.Step();
				}
			}
		}
	}

	private void Update()
	{
		HandleInput();
		if (!paused)
		{
			StepThroughMovers();
		}
	}

	static void Main()
	{
		new MyGame().Start();
	}
}