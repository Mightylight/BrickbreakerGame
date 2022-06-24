using System;
using System.Management.Instrumentation;
using GXPEngine;

public class Ball : EasyDraw
{
	// These four public static fields are changed from MyGame, based on key input (see Console):
	public static bool drawDebugLine = false;
	public static bool wordy = false;
	public static float bounciness = 0.98f;
	// For ease of testing / changing, we assume every ball has the same acceleration (gravity):
	public static Vec2 acceleration = new Vec2 (0, 0);
	
	
	public Vec2 velocity;
	public Vec2 position;

	public readonly int radius;
	public readonly bool moving;

	// Mass = density * volume.
	// In 2D, we assume volume = area (=all objects are assumed to have the same "depth")
	public float Mass {
		get {
			return radius * radius * _density;
		}
	}

	Vec2 _oldPosition;
	Arrow _velocityIndicator;

	float _density = 1;

	public Ball (int pRadius, Vec2 pPosition, Vec2 pVelocity=new Vec2(), bool moving=true) : base (pRadius*2 + 1, pRadius*2 + 1)
	{
		radius = pRadius;
		position = pPosition;
		velocity = pVelocity;
		this.moving = moving;

		position = pPosition;
		UpdateScreenPosition ();
		SetOrigin (radius, radius);

		Draw (230, 200, 0);

		_velocityIndicator = new Arrow(position, new Vec2(0,0), 10);
		AddChild(_velocityIndicator);
	}

	void Draw(byte red, byte green, byte blue) {
		Fill (red, green, blue);
		Stroke (red, green, blue);
		Ellipse (radius, radius, 2*radius, 2*radius);
	}

	void UpdateScreenPosition() {
		x = position.x;
		y = position.y;
	}

	public void Step () {
		velocity += acceleration;
		_oldPosition = position;
		position += velocity;
		// This can be removed after adding line segment collision detection:
		BoundaryWrapAround();			

		CollisionInfo firstCollision = FindEarliestCollision();
		if (firstCollision != null) {
			ResolveCollision(firstCollision);
		}

		UpdateScreenPosition();

		ShowDebugInfo();
	}

	CollisionInfo FindEarliestCollision() {
		MyGame myGame = (MyGame)game;
		CollisionInfo lowestToi = null;
		// Check other movers:			
		for (int i = 0; i < myGame.GetNumberOfMovers(); i++) {
			Ball mover = myGame.GetMover(i);
			if (mover != this)
			{
				CollisionInfo ballcheck = Ballcheck(mover);
				if (ballcheck != null)
				{
					if (lowestToi == null || lowestToi.timeOfImpact > ballcheck.timeOfImpact)
					{
						lowestToi = ballcheck;
					}
				}
			}
		}
		
		for (int i = 0; i < myGame.GetNumberOfCaps(); i++) {
			Ball mover = myGame.GetCaps(i);
			if (mover != this)
			{
				CollisionInfo ballcheck = Ballcheck(mover);
				if (ballcheck != null)
				{
					if (lowestToi == null || lowestToi.timeOfImpact > ballcheck.timeOfImpact)
					{
						lowestToi = ballcheck;
					}
				}
			}
		}
		
		for (int i = 0; i < myGame.GetNumberOfLines(); i++)
		{
			LineSegment l = myGame.GetLine(i);
			Vec2 diffVec = position - l.end;
			Vec2 lineVec = l.end - l.start;
			float ballDistance = lineVec.Normal().Dot(diffVec);
			if (ballDistance < radius)
			{
				Vec2 oldDiffVec = _oldPosition - l.end;
				float a = lineVec.Normal().Dot(oldDiffVec) - radius;
				float b = -lineVec.Normal().Dot(velocity);
				if (a >= 0 && b > 0)
				{
					float toi = a / b;
					Vec2 poi = _oldPosition + velocity * toi;
					Vec2 diffVec2 = l.end - poi;
					Vec2 unitVec = lineVec.Normalized();
					float distance = diffVec2.Dot(unitVec);
					if (toi <= 1)
					{
						if (distance >= 0 && distance < lineVec.Length())
						{
							CollisionInfo lineCheck = new CollisionInfo(lineVec.Normal(), l, toi);
							if (lowestToi == null || lowestToi.timeOfImpact > lineCheck.timeOfImpact)
							{
								lowestToi = lineCheck;
							}
						}
					}
				}
			}
		}
		return lowestToi;
	}

	private CollisionInfo Ballcheck(Ball mover)
	{
		Vec2 relativePosition = position - mover.position;
		if (relativePosition.Length () < radius + mover.radius) {
			Vec2 correctNormal = _oldPosition - mover.position;
			float a = Mathf.Pow(velocity.Length(),2);
			float b = (2 * correctNormal).Dot(velocity);
			float c = Mathf.Pow(correctNormal.Length(),2) - Mathf.Pow(radius + mover.radius,2);
			float d = Mathf.Pow(b, 2) - (4 * a * c);
			float toi = 0;
			if (d > 0)
			{
				toi = (-b - Mathf.Sqrt(d))/ (2 * a);
			}

			if (toi >= 0 && toi <= 1)
			{
				return new CollisionInfo(correctNormal, mover, toi);
			}
		}
		return null;
	}
	void ResolveCollision(CollisionInfo col) {
		Vec2 poi = _oldPosition + velocity * col.timeOfImpact;
		position = poi;
		velocity.Reflect(1,col.normal.Normal());
		velocity *= -1;
	}

	void BoundaryWrapAround() {
		if (position.x < 0) {
			position.x += game.width;
		}
		if (position.x > game.width) {			
			position.x -= game.width;
		}
		if (position.y < 0) {
			position.y += game.height;
		}
		if (position.y > game.height) {
			position.y -= game.height;
		}
	}

	void ShowDebugInfo() {
		if (drawDebugLine) {
			((MyGame)game).DrawLine (_oldPosition, position);
		}
		_velocityIndicator.startPoint = position;
		_velocityIndicator.vector = velocity;
	}
}

