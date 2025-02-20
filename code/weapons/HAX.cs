﻿using Sandbox;

[Library( "weapon_hax", Title = "HAX!", Spawnable = true )]
partial class hax : Weapon
{
	public override string ViewModelPath => "models/monitor_v.vmdl";
	public override float PrimaryRate => 1;
	public override float SecondaryRate => 1;
	public override float ReloadTime => 0.5f;

	public override void Spawn()
	{
		base.Spawn();
	
		SetModel("models/monitor.vmdl");
	}
public override void AttackPrimary()
	{
		(Owner as AnimEntity)?.SetAnimBool("b_attack", true);
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;
		TimeSince timeSinceShoot;

		//(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hax" );

		//
		// Shoot the bullets
		//
		//ShootBullets( 10, 0.1f, 10.0f, 9.0f, 3.0f );

		ShootBox();
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = -0.5f;
		TimeSinceSecondaryAttack = -0.5f;

		//(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		DoubleShootEffects();
		PlaySound("hax");

		//
		// Shoot the bullets
		//
		//ShootBullets( 20, 0.4f, 20.0f, 8.0f, 3.0f );
		ShootBox();
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		//Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		//Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection_point" );

		ViewModelEntity?.SetAnimBool( "fire", true );

		if ( IsLocalPawn )
		{
			new Sandbox.ScreenShake.Perlin( 1.0f, 1.5f, 2.0f );
		}

		CrosshairPanel?.CreateEvent( "fire" );
	}

	[ClientRpc]
	protected virtual void DoubleShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );

		ViewModelEntity?.SetAnimBool( "fire_double", true );
		CrosshairPanel?.CreateEvent( "fire" );

		if ( IsLocalPawn )
		{
			new Sandbox.ScreenShake.Perlin( 3.0f, 3.0f, 3.0f );
		}
	}

	public override void OnReloadFinish()
	{
		IsReloading = false;

		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		FinishReload();
	}

	[ClientRpc]
	protected virtual void FinishReload()
	{
		//ViewModelEntity?.SetAnimBool( "reload_finished", true );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 4 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
		anim.SetParam("holdtype_handedness", 0);
		anim.SetParam("holdtype_pose", 2.25f);
		anim.SetParam("holdtype_pose_hand", 0.01f);
	}

	void ShootBox()
	{
		if (IsClient) return;
		var ent = new Prop
		{
			Position = Owner.EyePos + Owner.EyeRot.Forward * 50,
			Rotation = Owner.EyeRot
		};

		ent.SetModel("models/monitor.vmdl");
		ent.Velocity = Owner.EyeRot.Forward * 10000;
	}
}
