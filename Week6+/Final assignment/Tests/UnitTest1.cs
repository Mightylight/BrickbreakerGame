using System;
using System.Drawing.Imaging;
using System.Reflection;
using GXPEngine;
using NUnit.Framework;

namespace Tests;

public class Tests
{
    public void AssertVector(Vec2 expected, Vec2 compared)
    {
        Assert.AreEqual(expected.x,compared.x, 0.1f);
        Assert.AreEqual(expected.y, compared.y, 0.1f);
    }
    
    [Test]
    public void TestDeg2Rad()
    {
        void TestNormalizedSingular(float deg, float rad)
		{
			Assert.AreEqual(Vec2.Deg2Rad(deg), rad, 0.01f);
			Assert.AreEqual(Vec2.Rad2Deg(rad), deg, 0.01f);
		}

		TestNormalizedSingular(0, 0);
		TestNormalizedSingular(180, Mathf.PI);
		TestNormalizedSingular(360, 2 * Mathf.PI);
		TestNormalizedSingular(-180, -Mathf.PI);
		TestNormalizedSingular(-360, 2 * -Mathf.PI);
    }

    [Test]
    public void TestSetAngleDegrees()
    {
        AssertVector(new Vec2(7.07f,7.07f), new Vec2(10f,0).SetAngleDegrees(45));
        AssertVector(new Vec2(1, 0), new Vec2(1, 0).SetAngleDegrees(0));
        AssertVector(new Vec2(0, 1), new Vec2(1, 0).SetAngleDegrees(90));
        AssertVector(new Vec2(0, 10), new Vec2(0, -10).SetAngleDegrees(90));
        AssertVector(new Vec2(-10, 0), new Vec2(0, -10).SetAngleDegrees(180));
        AssertVector(new Vec2(0, -10), new Vec2(0, -10).SetAngleDegrees(-90));
        AssertVector(new Vec2(0, 10), new Vec2(0, -10).SetAngleDegrees(90+360));
    }

    [Test]
    public void TestSetAngleRadians()
    {
        AssertVector(new Vec2(7.07f,7.07f), new Vec2(10f,0).SetAngleRadians(0.25f * Mathf.PI));
        AssertVector(new Vec2(1, 0), new Vec2(1, 0).SetAngleRadians(0));
        AssertVector(new Vec2(0, 1), new Vec2(1, 0).SetAngleRadians(Mathf.PI/2));
        AssertVector(new Vec2(0, 10), new Vec2(0, -10).SetAngleRadians(Mathf.PI/2));
        AssertVector(new Vec2(-10, 0), new Vec2(0, -10).SetAngleRadians(Mathf.PI));
        AssertVector(new Vec2(0, -10), new Vec2(0, -10).SetAngleRadians(-Mathf.PI/2));
        AssertVector(new Vec2(0, 10), new Vec2(0, -10).SetAngleRadians(Mathf.PI/2 + Mathf.PI * 2));
    }

    [Test]
    public void TestGetDegrees()
    { 
        Assert.AreEqual(0, new Vec2(0, 0).GetAngleDegrees());
        Assert.AreEqual(0, new Vec2(1, 0).GetAngleDegrees());
        Assert.AreEqual(90, new Vec2(0, 1).GetAngleDegrees());
        Assert.AreEqual(180, new Vec2(-1, 0).GetAngleDegrees());
    }

    [Test]
    public void TestGetRadians()
    {
        Assert.AreEqual(0, new Vec2(0, 0).GetAngleRadians());
        Assert.AreEqual(0, new Vec2(1, 0).GetAngleRadians());
        Assert.AreEqual(Mathf.PI/2, new Vec2(0, 1).GetAngleRadians());
        Assert.AreEqual(Mathf.PI, new Vec2(-1, 0).GetAngleRadians());
    }

    [Test]
    public void TestRotateDegrees()
    {
        AssertVector(new Vec2(-5f,2f),new Vec2(2f,5f).RotateDegrees(90));
        AssertVector(new Vec2(0, 0), new Vec2(0, 0).RotateDegrees(0));
        AssertVector(new Vec2(-1, 0), new Vec2(1, 0).RotateDegrees(180));
    }

    [Test]
    public void TestRotateRadians()
    {
        AssertVector(new Vec2(-5f , 2f), new Vec2(2f, 5f).RotateRadians(0.5f * Mathf.PI));
        AssertVector(new Vec2(0, 0), new Vec2(0, 0).RotateRadians(0));
        AssertVector(new Vec2(-1, 0), new Vec2(1, 0).RotateRadians(Mathf.PI));
    }

    [Test]
    public void TestRotateAroundRadians()
    {
        AssertVector(new Vec2(-3f,3f),new Vec2(4f,6f).RotateAroundRadians(0.5f * Mathf.PI, new Vec2(2f,1f)));
    }
    
    [Test]
    public void TestRotateAroundDegrees()
    {
        AssertVector(new Vec2(-3f,3f),new Vec2(4f,6f).RotateAroundDegrees(90, new Vec2(2f,1f)));
    }

    [Test]
    public void TestGetUnitVectorDegrees()
    {
        AssertVector(new Vec2(1f, 0f), Vec2.GetUnitVectorDeg(0));
        AssertVector(new Vec2(-1f, 0), Vec2.GetUnitVectorDeg(180f));                       
        AssertVector(new Vec2(0, -1f), Vec2.GetUnitVectorDeg(270f));          
        AssertVector(new Vec2(0.7f, 0.7f), Vec2.GetUnitVectorDeg(45f));
        AssertVector(new Vec2(0.7f, -0.7f), Vec2.GetUnitVectorDeg(-45f));
    }
    
    [Test]
    public void TestGetUnitVectorRadians()
    {
        AssertVector(new Vec2(1f,0f),Vec2.GetUnitVectorRad(0));
        AssertVector(new Vec2(-1, 0), Vec2.GetUnitVectorRad(Mathf.PI));
        AssertVector(new Vec2(0, -1), Vec2.GetUnitVectorRad(Mathf.PI + Mathf.PI/2));
        AssertVector(new Vec2(0.7f, 0.7f), Vec2.GetUnitVectorRad(Mathf.PI/4));
        AssertVector(new Vec2(0.7f, -0.7f), Vec2.GetUnitVectorRad(-Mathf.PI/4));
    }

    [Test]
    public void TestRandomUnitVector()
    {
        int posx = 0;
        int negx = 0;
        int posy = 0;
        int negy = 0;
        for (int i = 0; i < 100; i++)
        {
            Vec2 randomVector = Vec2.RandomUnitVector();
            Assert.AreEqual(randomVector.Length(),1.0f,0.01f);
            Console.WriteLine(randomVector);
            if (randomVector.x > 0) posx++;
            else negx++;
            if (randomVector.y > 0) posy++;
            else negy++;
        }

        Console.WriteLine("posx: {0} negx: {1}  posy: {2} negy: {3}",posx,negx,posy,negy);
        //Assert.Ignore();
    }

    [Test]
    public void TestNormalize()
    {
        Vec2 n = new Vec2(6, 8);
        n.Normalize();
        AssertVector(new Vec2(0.6f,0.8f),n);
        //AssertVector(new Vec2(-0.6f, 0.8f),new Vec2(-3f, 4f).Normalize());
        //AssertVector(new Vec2(0, 1f),new Vec2(0, 4f).Normalize());
        //AssertVector(new Vec2(0, 0),new Vec2(0, 0).Normalize());
        //AssertVector(new Vec2(1f, 0),new Vec2(0.5f, 0).Normalize());
    }

    [Test]
    public void TestNormalized()
    {
        AssertVector(new Vec2(0.6f,0.8f), new Vec2(6f,8f).Normalized());
        AssertVector(new Vec2(-0.6f, 0.8f), new Vec2(-3f, 4f).Normalized());
        AssertVector(new Vec2(0, 1f),new Vec2(0, 4f).Normalized());   
        AssertVector(new Vec2(0, 0),new Vec2(0, 0).Normalized());     
        AssertVector(new Vec2(1f, 0),new Vec2(0.5f, 0).Normalized()); 
    }

    [Test]
    public void TestSubtraction()
    {
        AssertVector(new Vec2(-4f,-5f), new Vec2(2f,3f)- new Vec2(6f,8f));
        AssertVector(new Vec2(-1f, -1f), new Vec2(1f, 1f) - new Vec2(2f, 2f));
        AssertVector(new Vec2(-5f, 3f), new Vec2(0f, 1f) - new Vec2(5f, -2f));
        AssertVector(new Vec2(-0.9f, -51f), new Vec2(-1f, -1f) - new Vec2(-0.1f, 50f));
    }

    [Test]
    public void TestMultiplication()
    {
        AssertVector(new Vec2(6f,9f), new Vec2(2f,3f)* 3f);
        AssertVector(new Vec2(8f,12f), 4 * new Vec2(2f,3f));
        AssertVector(new Vec2(-6f,9f), new Vec2(-2f,3f)* 3f);
        AssertVector(new Vec2(8f,-12f), 4 * new Vec2(2f,-3f));
    }

    [Test]
    public void TestLength()
    {
        Assert.AreEqual(10f,new Vec2(6f,8f).Length(),0.01f);
        Assert.AreEqual(1, new Vec2(0, 1).Length(), 0.01f);
        Assert.AreEqual(1, new Vec2(0, -1).Length(), 0.01f);
        Assert.AreEqual(5, new Vec2(-3, -4).Length(), 0.01f);
        Assert.AreEqual(4, new Vec2(0, 4).Length(), 0.01f);
    }

    [Test]
    public void TestAddition()
    {
        AssertVector(new Vec2(4f,5f), new Vec2(1f,3f) + new Vec2(3f, 2f));
    }

    [Test]
    public void TestSetXY()
    {
        AssertVector(new Vec2(4f,5f), new Vec2(1f,2f).SetXy(4f,5f));
        AssertVector(new Vec2(1,0), new Vec2().SetXy(1, 0));
        AssertVector(new Vec2(10,0), new Vec2().SetXy(10, 0));
        AssertVector(new Vec2(-10,0), new Vec2().SetXy(-10, 0));
        AssertVector(new Vec2(0,1), new Vec2().SetXy(0,1));
        AssertVector(new Vec2(0,10), new Vec2().SetXy(0,10));
        AssertVector(new Vec2(0,-10), new Vec2().SetXy(0,-10));
    }
}