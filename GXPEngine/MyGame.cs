using System;
using System.Collections.Generic; // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;							// System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{
	private List<Ball> _movers;
	private List<Ball> _caps;
	private List<LineSegment> _lines;
	private Canvas _lineContainer;

	public MyGame() : base(800, 600, false)		// Create a window that's 800x600 and NOT fullscreen
	{
		_movers = new List<Ball>();
		Console.WriteLine("MyGame initialized");
		AddLine (new Vec2 (width-60, height-60), new Vec2 (50, height-20));
		AddLine (new Vec2 (50, height-20), new Vec2 (200, 60));
		AddLine (new Vec2 (200, 60), new Vec2 (width-20, 50));
		AddLine (new Vec2 (width-20, 50), new Vec2 (width-60, height-60));
	}
	
	void Update()
	{
		
	}
	public int GetNumberOfLines() {
		return _lines.Count;
	}

	public LineSegment GetLine(int index) {
		if (index >= 0 && index < _lines.Count) {
			return _lines [index];
		}
		return null;	
	}

	public int GetNumberOfMovers() {
		return _movers.Count;
	}

	public Ball GetMover(int index) {
		if (index >= 0 && index < _movers.Count) {
			return _movers [index];
		}
		return null;
	}

	public int GetNumberOfCaps()
	{
		return _caps.Count;
	}

	public Ball GetCaps(int index)
	{
		if (index >= 0 && index < _caps.Count) {
			return _caps [index];
		}
		return null;
	}
	
	public void DrawLine(Vec2 start, Vec2 end) {
		_lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
	}
	
	void AddLine (Vec2 start, Vec2 end) {
		LineSegment line = new LineSegment (start, end, 0xff00ff00, 4);
		AddChild (line);
		_lines.Add (line);
		LineSegment lineOpp = new LineSegment (end, start, 0xff00ff00, 4);
		AddChild (lineOpp);
		_lines.Add (lineOpp);
		Ball endcap = new Ball(0, end);
		Ball startcap = new Ball(0, start);
		_caps.Add(startcap);
		_caps.Add(endcap);
		AddChild(startcap);
		AddChild(endcap);
	}

	static void Main()
	{
		new MyGame().Start();
	}
}