using Sandbox;
using System;

[Library( "ent_creepydoll", Title = "Creepy Doll", Spawnable = true )]
public partial class CreepyDollEntity : Prop, IUse
{
	public float MaxSpeed { get; set; } = 1000.0f;
	public float SpeedMul { get; set; } = 1.5f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/props_c17/doll01.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		Scale = Rand.Float( 1f, 2f );
		//RenderColor = Color.Random.ToColor32();
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		var speed = eventData.PreVelocity.Length;
		var direction = Vector3.Reflect( eventData.PreVelocity.Normal, eventData.Normal.Normal ).Normal;
		Velocity = direction * MathF.Min( speed * SpeedMul, MaxSpeed );
		Sound.FromEntity("teddy", this);
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	public bool OnUse( Entity user )
	{
		if ( user is Player player )
		{
			player.Health += 10;
			PlaySound("teddy");

			Delete();
		}

		return false;
	}
}
