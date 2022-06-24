using System;
using GXPEngine;

// Allows using Mathf functions
namespace GXPEngine.core
{


    public struct Vec2
    {
        public float x;
        public float y;

        public Vec2(float pX = 0, float pY = 0)
        {
            x = pX;
            y = pY;
        }

        public Vec2(Vec2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public float Length()
        {
            return Mathf.Sqrt(x * x + y * y);
        }

        public static Vec2 operator +(Vec2 left, Vec2 right)
        {
            return new Vec2(left.x + right.x, left.y + right.y);
        }

        public static Vec2 operator -(Vec2 left, Vec2 right)
        {
            return new Vec2(left.x - right.x, left.y - right.y);
        }

        public static Vec2 operator *(float factor, Vec2 right)
        {
            return new Vec2(factor * right.x, factor * right.y);
        }

        public static Vec2 operator *(Vec2 left, float factor)
        {
            return new Vec2(factor * left.x, factor * left.y);
        }

        public static float Rad2Deg(float radians)
        {
            return radians * 180 / Mathf.PI;
        }

        public static float Deg2Rad(float degrees)
        {
            
            return (float) (Math.PI / 180) * degrees;
        }

        public static Vec2 GetUnitVectorRad(float radians)
        {
            return new Vec2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public static Vec2 GetUnitVectorDeg(float degrees)
        {
            return new Vec2(GetUnitVectorRad(degrees));
        }

        //public static Vec2 RandomUnitVector()
        //{

        //}


        public Vec2 Normalized()
        {
            var length = Length();
            var pX = x;
            var pY = y;
            if (!(length > 0)) return new Vec2(pX, pY);
            pX /= length;
            pY /= length;
            return new Vec2(pX, pY);
        }

        public void Normalize()
        {
            var length = Length();
            if (!(length > 0)) return;
            x /= length;
            y /= length;
        }

        public void SetXY(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2 SetAngleRadians(float radians)
        {
            var length = Length();
            x = Mathf.Cos(radians) * length;
            y = Mathf.Sin(radians) * length;
            return this;
        }

        public float GetAngleRadians(Vec2 vector)
        {
            return Mathf.Atan2(vector.x, vector.y);
        }

        public Vec2 SetAngleDegrees(float degrees)
        {
            return SetAngleRadians(Deg2Rad(degrees));
        }

        public float GetAngleDegrees(Vec2 vector)
        {
            return Rad2Deg(GetAngleRadians(vector));
        }

        public void RotateDegrees(float degrees)
        {
            var vector = new Vec2(x, y);
            var angleDegrees = GetAngleDegrees(vector);
            SetAngleDegrees(angleDegrees += degrees);
        }

        public void RotateRadians(float radians)
        {
            RotateDegrees(Rad2Deg(radians));
        }

        public void RotateAroundDegrees(float degrees, Vec2 point)
        {
            this -= point;
            RotateDegrees(degrees);
            this += point;
        }

        public void RotateAroundRadians(float radians, Vec2 point)
        {
            RotateAroundDegrees(Rad2Deg(radians), point);
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }
    }
}