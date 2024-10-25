using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	
    public Timer Timer; //timer for damage bar to disappear
    public ProgressBar DamageBar; //shows how much damage player took
    
    public int Health = 0;

    public override void _Ready()
    {
        Timer = GetNode<Timer>("Timer");
        DamageBar = GetNode<ProgressBar>("DamageBar");
    }
	
	//updates health when taking damage
    public void SetHealth(int newHealth)
    {
        int prevHealth = Health;
        Health = (int)Mathf.Min(MaxValue, newHealth);
        Value = Health;
        
        if (Health <= 0)
        {
			QueueFree();
        }

        if (Health < prevHealth)
        {
            Timer.Start(); //starts timer for damage bar to appear
        }
        else
        {
            DamageBar.Value = Health;
        }
    }
	//initializes health
    public void InitHealth(int initialHealth)
    {
        Health = initialHealth;
        MaxValue = Health;
        Value = Health;
        DamageBar.MaxValue = Health;
        DamageBar.Value = Health;
    }
}