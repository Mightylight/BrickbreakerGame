using GXPEngine.Components;

namespace GXPEngine;

public class Ball : EasyDraw
{
	// These four public static fields are changed from MyGame, based on key input (see Console):
	public static bool drawDebugLine = false;
	public static bool wordy = false;
	public static float bounciness = 0.98f;
	// For ease of testing / changing, we assume every ball has the same acceleration (gravity):
	public static Vec2 acceleration = new Vec2 (0, 0);


	private Vec2 velocity;
	private Vec2 position;

	private readonly int radius;
	public readonly bool moving;
	private readonly int maxDistance;

	private Vec2 oldPosition;
	private readonly GameObject owner;

	public Ball (int pRadius, Vec2 pPosition, GameObject owner, Vec2 pVelocity=new Vec2(), bool moving=true) : base (pRadius*2 + 1, pRadius*2 + 1)
	{
		radius = pRadius;
		position = pPosition;
		velocity = pVelocity;
		this.moving = moving;
		this.owner = owner;
		position = pPosition;
		maxDistance = 50;
		UpdateScreenPosition ();
		SetOrigin (radius, radius);
		Draw (230, 200, 0);
	}

	private void Draw(byte red, byte green, byte blue) {
		Fill (red, green, blue);
		Stroke (red, green, blue);
		Ellipse (radius, radius, 2*radius, 2*radius);
	}

	private void UpdateScreenPosition() {
		x = position.x;
		y = position.y;
	}

	public void Step () {
		velocity += acceleration;
		oldPosition = position;
		position += velocity;
		BoundaryDetection();			

		CollisionInfo firstCollision = FindEarliestCollision();
		if (firstCollision != null) {
			ResolveCollision(firstCollision);
		}

		UpdateScreenPosition();

		ShowDebugInfo();
	}

	private CollisionInfo FindEarliestCollision() {
		MyGame myGame = (MyGame)game;
		CollisionInfo lowestToi = null;
		if (myGame.bulletCollide) //ball collision (can be disabled)
		{
			for (int i = 0; i < myGame.GetNumberOfMovers(); i++)
			{
				Ball mover = myGame.GetMover(i);
				if (mover == this) continue;
				CollisionInfo ballcheck = Ballcheck(mover);
				if (ballcheck == null) continue;
				if (lowestToi == null || lowestToi.timeOfImpact > ballcheck.timeOfImpact)
				{
					lowestToi = ballcheck;
				}
			}
		}

		foreach (Triangle t in myGame.triangles)  //caps collision
		{
			for (int i = 0; i < t.GetNumberOfCaps(); i++)
			{
				Ball mover = t.GetCaps(i);
				if (mover == this) continue;
				//special check for performance reasons, seeing how far they are away from the ball before calculating
				if(mover.position.x < position.x - maxDistance || mover.position.y < position.y - maxDistance ||
				   mover.position.x > position.x + maxDistance || mover.position.y > position.y + maxDistance) continue;
					
				CollisionInfo ballcheck = Ballcheck(mover);
				if (ballcheck == null) continue;
				if (lowestToi == null || lowestToi.timeOfImpact > ballcheck.timeOfImpact)
				{
					lowestToi = ballcheck;
				}
				
			}

			for (int i = 0; i < t.GetNumberOfLines(); i++) //line collisions
			{
				LineSegment l = t.GetLine(i);
				Triangle g = (Triangle) l.owner;
				//special check for performance reasons, seeing how far they are away from the ball before calculating
				if (g.position.x < position.x - maxDistance || g.position.x > position.x + maxDistance ||		
				    g.position.y < position.y - maxDistance || g.position.y > position.y + maxDistance) continue;
				Vec2 diffVec = position - l.end;
				Vec2 lineVec = l.end - l.start;
				float ballDistance = lineVec.Normal().Dot(diffVec);
				if (!(ballDistance < radius)) continue;
				Vec2 oldDiffVec = oldPosition - l.end;
				float a = lineVec.Normal().Dot(oldDiffVec) - radius;
				float b = -lineVec.Normal().Dot(velocity);
				if (!(a >= 0) || !(b > 0)) continue;
				float toi = a / b;
				Vec2 poi = oldPosition + velocity * toi;
				Vec2 diffVec2 = l.end - poi;
				Vec2 unitVec = lineVec.Normalized();
				float distance = diffVec2.Dot(unitVec);
				if (!(toi <= 1)) continue;
				if (!(distance >= 0) || !(distance < lineVec.Length())) continue;
				CollisionInfo lineCheck = new(lineVec.Normal(), l, toi);
				if (lowestToi == null || lowestToi.timeOfImpact > lineCheck.timeOfImpact)
				{
					lowestToi = lineCheck;
				}
			}
		}
		return lowestToi;
	}

	private CollisionInfo Ballcheck(Ball mover)
	{
		Vec2 correctNormal = oldPosition - mover.position;
		float a = Mathf.Pow(velocity.Length(),2);
		if (a == 0) return null;
		float b = (2 * correctNormal).Dot(velocity);
		float c = Mathf.Pow(correctNormal.Length(),2) - Mathf.Pow(radius + mover.radius,2);
		float d = Mathf.Pow(b, 2) - (4 * a * c);
		float toi;
		if (d < 0) return null;
		if (c < 0)
		{
			if (b < 0 || (a > -radius && a < 0))
			{
				toi = 0;
			}
			else return null;
		}
		else
		{
			toi = (-b - Mathf.Sqrt(d))/ (2 * a);
		}


		return toi is < 0 or > 1 ? null : new CollisionInfo(correctNormal.Normalized(), mover, toi);
	}
	private void ResolveCollision(CollisionInfo col) {
		Vec2 poi = oldPosition + velocity * col.timeOfImpact;
		position = poi;
		velocity.Reflect(1,col.normal);
		//velocity *= -1;

		switch (col.other)
		{
			case LineSegment l:
			{
				if (l.owner is Triangle t)
				{
					t.Hit();
				}

				break;
			}
			case Ball b:
			{
				if (b.owner is Triangle t)
				{
					t.Hit();
				}

				break;
			}
		}
	}

	private void BoundaryDetection() {
		if (position.x < radius) {
			velocity.x *= -1;
		}
		if (position.x > game.width + radius) {			
			velocity.x *= -1;
		}
		if (position.y < radius) {
			velocity.y *= -1;
		}
		if (position.y > game.height + radius) {
			LateDestroy();
		}
	}

	private void ShowDebugInfo() {
		if (drawDebugLine) {
			((MyGame)game).DrawLine (oldPosition, position);
		}
	}

	protected override void OnDestroy()
	{
		MyGame myGame = (MyGame) game;
		myGame.movers.Remove(this);
	}
}