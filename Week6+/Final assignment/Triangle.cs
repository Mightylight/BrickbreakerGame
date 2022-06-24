using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GXPEngine.Components;

namespace GXPEngine;

public class Triangle : GameObject
{
    private int health = 3;
    private readonly List<LineSegment> lines;
    private readonly List<Ball> caps;
    public Vec2 position;
    private readonly Vec2 point1;
    private readonly Vec2 point2;
    private readonly Vec2 point3;
    private readonly Vec2 point4;
    public Triangle(float x, float y, int o)
    {
        lines = new List<LineSegment>();
        caps = new List<Ball>();
        position = new Vec2(x, y);
        this.x = x;
        this.y = y;
        point1 = new Vec2(0,0); 
        point2 = new Vec2(0, 30); 
        point3 = new Vec2(30, 30);
        point4 = new Vec2(30, 0);
        OrientTriangle(o);
    }


    private void OrientTriangle(int o)
    {
        switch (o)
        {
            case 0 :
                AddLine(point1,point2);
                AddLine(point2,point3);
                AddLine(point3,point1);
                break;
            case 1 :
                AddLine(point2,point3);
                AddLine(point3,point4);
                AddLine(point4,point2);
                break;
            case 2 :
                AddLine(point1,point2);
                AddLine(point2,point4);
                AddLine(point4,point1);
                break;
            case 3 :
                AddLine(point1,point3);
                AddLine(point3,point4);
                AddLine(point4,point1);
                break;
            case 4:
                AddLine(point1,point2);
                AddLine(point2,point3);
                AddLine(point3,point4);
                AddLine(point4,point1);
                break;
        }
    }

    private void AddLine (Vec2 start, Vec2 end) {
        LineSegment line = new(start + position, end + position,this , 0xff00ff00, 3);
        lines.Add (line);
        LineSegment lineOpp = new(end + position, start + position,this,  0xff00ff00, 3);
        lines.Add (lineOpp);
        Ball endcap = new(0, end + position, this);
        caps.Add(endcap);
        foreach (LineSegment l in lines)
        {
            Game.main.AddChild(l);
        }

        foreach (Ball c in caps)
        {
            Game.main.AddChild(c);
        }
    }
    public int GetNumberOfLines() {
        return lines.Count;
    }
    
    public LineSegment GetLine(int index) {
        if (index >= 0 && index < lines.Count) {
            return lines [index];
        }
        return null;	
    }
    
    public int GetNumberOfCaps()
    {
        return caps.Count;
    }

    public Ball GetCaps(int index)
    {
        if (index >= 0 && index < caps.Count) {
            return caps [index];
        }
        return null;
    }

    public void Hit()
    {
        health--;
        if (health > 0) return;
        foreach (Ball c in caps)
        {
            c.Destroy();
        }
        caps.Clear();
        foreach (LineSegment l in lines)
        {
            l.Destroy();
        }
        lines.Clear();
        LateDestroy();
    }
    
    protected override void OnDestroy()
    {
        MyGame myGame = (MyGame) game;
        myGame.triangles.Remove(this);
    }
}