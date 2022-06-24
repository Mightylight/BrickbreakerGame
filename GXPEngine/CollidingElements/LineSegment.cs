using System;
using GXPEngine.Core;
using GXPEngine.OpenGL;

namespace GXPEngine
{
    /// <summary>
    /// Implements an OpenGL line
    /// </summary>
    public class LineSegment : GameObject
    {
        public Vec2 start;
        public Vec2 end;

        public uint color = 0xffffffff;
        public uint lineWidth = 1;

        public LineSegment (float pStartX, float pStartY, float pEndX, float pEndY, uint pColor = 0xffffffff, uint pLineWidth = 1)
            : this (new Vec2 (pStartX, pStartY), new Vec2 (pEndX, pEndY), pColor, pLineWidth)
        {
        }

        public LineSegment (Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1)
        {
            start = pStart;
            end = pEnd;
            color = pColor;
            lineWidth = pLineWidth;
        }
	
        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        override protected void RenderSelf(GLContext glContext) {
            if (game != null) {
                Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
            }
        }
    }
}