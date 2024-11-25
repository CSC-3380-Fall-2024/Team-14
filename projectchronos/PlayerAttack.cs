using System.Numerics;
using Godot;

public partial class PlayerAttack : Area2D
{
	// internal base values,
	// could be changed for entire new attacks
	private int damage;
	private float attacksPerSecond;
	private float immunityDuration;

	// for modifier upgrades
	private float damageModifier = 1f;
	private float rateModifier = 1f;

	[Export]
	public int defaultDamage = 5;
	[Export]
	public float defaultAttacksPerSecond = 3;
	[Export]
	public float defaultImmunityDuration = 0.1f;

	// we use a timer node for handling attack rate
	private Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		damage = defaultDamage;
		attacksPerSecond = defaultAttacksPerSecond;
		immunityDuration = defaultImmunityDuration;

		// find the timer and set default rate
		timer = GetChild<Timer>(1);
		timer.SetWaitTime((double) AttackPeriod());
	}

	// setting attack frequency is more intuitive for design
	// but attack period is more useful for implementation
	public float AttackPeriod() {
		return 1 / attacksPerSecond;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (Input.IsActionPressed("click")) {
			if (timer.TimeLeft <= 0.0) {
				Attack();
				timer.Start(AttackPeriod());
			}
		} else {
			Rotation = 0;
		}
	}

	public void SetAttack(int damage, float attacksPerSecond, float immunityDuration) {
		SetDamage(damage);
		SetAttacksPerSecond(attacksPerSecond);
		SetImmunityDuration(immunityDuration);
	}

	public void SetDamage(int damage) {
		this.damage = damage;
	}

	public void SetAttacksPerSecond(float attacksPerSecond) {
		this.attacksPerSecond = attacksPerSecond;
	}

	public void SetImmunityDuration(float immunityDuration) {
		this.immunityDuration = immunityDuration;
	}

	// ignoring enemy immunity for a potent upgrade
	public void IgnoreImmunity() {
		SetImmunityDuration(0f);
	}

	// we don't want to modify our damage value directly, we keep it and scale by an upgrade factor
	public void AddDamageModifier(float damagePercent) {
		damageModifier += damagePercent / 100;
	}

	// we also don't want to modify attack rate either, so we keep it and scale with upgrades
	public void AddRateModifier(float ratePercent) {
		rateModifier += ratePercent / 100;
	}

	// actual damage after applying any upgrades
	public int ScaledDamage() {
		return (int) (damageModifier * damage);
	}

	// attack rate after upgrades
	public float ScaledRate() {
		return attacksPerSecond * rateModifier;
	}

	// DO NOT USE FOR ACTUAL GAMEPLAY MECHANICS
	// this method calculates effective rate when accounting for immunity frames
	// it's for if we want to tell the (human) player, not actual implementation
	public float EffectiveRate() {
		if (immunityDuration > ScaledRate()) {
			return 1 / immunityDuration;
		} else {
			return ScaledRate();
		}
	}

	// this is only for if we want to tell the (human) player what their theoretical DPS is
	public float CalculatedDPS() {
		return EffectiveRate() * ScaledDamage();
	}

	// actual attack, enemies should probably handle the taking damage bit?
	public void Attack() {
		LookAt(GetGlobalMousePosition());	
	}
}
