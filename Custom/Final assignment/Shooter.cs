using System.Threading;

namespace GXPEngine;

internal class Shooter : EasyDraw
{
    private const float ROTATIONSPEEDMAX = 10f;
    private readonly Vec2 position = new (Game.main.width / 2, Game.main.height);
    public Shooter() : base(100,100)
    {
        SetXY(position.x,position.y-50);
        SetOrigin(width/2,height/2);
        SetColor(0,255,0);
        Fill(0,255,0);
        Clear(50);
    }

    public void Update()
    {
        rotation += Mathf.Clamp(AngleDifference(), -ROTATIONSPEEDMAX, ROTATIONSPEEDMAX);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    private float AngleDifference()
    {
        Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY) - position;
        float sum = mousePos.GetAngleDegrees() - WarpAroundDegree(rotation);
        float diff = (sum + 180) % 360 - 180;
        return diff < -180 ? diff + 360 : diff;
    }
	
    private void Shoot()
    {
        MyGame myGame = (MyGame) game;
        Vec2 direction = Vec2.GetUnitVectorDeg(rotation);
        Ball bullet = new (6,position, this, direction * 10);
        myGame.movers.Add(bullet);
        myGame.AddChild(bullet);
    }

    /// <summary>
    /// Returns an equivalent angle that is always between 0 and 360
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static float WarpAroundDegree(float angle)
    {
        return((angle % 360) + 360) % 360;
    }
}