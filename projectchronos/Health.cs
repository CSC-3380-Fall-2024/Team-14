using Godot;
using System;

public partial class Health : Node
{
	[Signal]
	public delegate void MaxHealthChangedEventHandler(int diff);

	[Signal]
	public delegate void HealthChangedEventHandler(int diff);

	[Signal]
	public delegate void HealthDepletedEventHandler();

	[Export]
	public int MaxHealth { get; set; } = 20;

	private int health;

	public override void _Ready()
	{
		health = MaxHealth;
	}

	public void SetMaxHealth(int value)
	{
		int clampedValue = value <= 0 ? 1 : value; //make sure max health is not less than 1

		if (clampedValue != MaxHealth) //checks if health changed
		{
			int difference = clampedValue - MaxHealth;
			MaxHealth = value;
			EmitSignal(nameof(MaxHealthChanged), difference);

			if (health > MaxHealth) //makes sure health does not exceed max
			{
				health = MaxHealth;
			}
		}
	}

	public int GetMaxHealth()
	{
		return MaxHealth;
	}

   
   

	public void SetHealth(int value)
	{
		if (value < health)
		{
			return;
		}

		int clampedValue = Mathf.Clamp(value, 0, MaxHealth);

		if (clampedValue != health)
		{
			int difference = clampedValue - health;
			health = value;
			EmitSignal(nameof(HealthChanged), difference);

			if (health == 0)
			{
				EmitSignal(nameof(HealthDepleted));
			}
		}
	}

	public int GetHealth()
	{
		return health;
	}
}
