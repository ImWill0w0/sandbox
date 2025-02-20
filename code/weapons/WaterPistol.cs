﻿using Sandbox;

[Library("weapon_waterpistol", Title = "Water Pistol", Spawnable = true)]
partial class WaterPistol : Weapon
{
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 1.0f;

	public TimeSince TimeSinceDischarge { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel("weapons/rust_pistol/rust_pistol.vmdl");
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed(InputButton.Attack1);
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		//(Owner as AnimEntity)?.SetAnimBool("b_attack", true);

		ShootEffects();
		PlaySound("squirt");
		//ShootBullet(0.05f, 1.5f, 9.0f, 3.0f);
	}

	private void Discharge()
	{
		if ( TimeSinceDischarge < 0.5f )
			return;

		TimeSinceDischarge = 0;

		var muzzle = GetAttachment( "muzzle" ) ?? default;
		var pos = muzzle.Position;
		var rot = muzzle.Rotation;

		ShootEffects();
		PlaySound( "rust_pistol.shoot" );
		//ShootBullet( pos, rot.Forward, 0.05f, 1.5f, 9.0f, 3.0f );

		ApplyAbsoluteImpulse( rot.Backward * 200.0f );
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create("particles/watergun_squirt.vpcf", EffectEntity, "muzzle");

		//ViewModelEntity?.SetAnimBool("fire", true);

		CrosshairPanel?.CreateEvent("fire");
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		if ( eventData.Speed > 500.0f )
		{
			Discharge();
		}
	}
}
