using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public partial class Modifiers : Control
{
public class Upgrade
{
	public string UpgradeName { get; set; }
	public string Description { get; set; }

	public Upgrade(string name, string description)
	{
		UpgradeName = name;
		Description = description;
	}
}
	 public List<Upgrade> allUpgrades = new List<Upgrade>
	{
		new Upgrade("Double Jump", "Allows you to jump twice in mid-air."),
		new Upgrade("Speed Boost", "Increases your movement speed."),
		new Upgrade("Health Regen", "Regenerates health over time."),
		new Upgrade("Increased Damage", "Increases your attack damage."),
		new Upgrade("Shield", "Gives you a temporary shield."),
		new Upgrade("Magnet", "Attracts nearby items."),
		new Upgrade("Fireball", "Shoots a fireball every few seconds."),
		new Upgrade("Dash", "Allows you to dash short distances.")
	};

	private Button UpgradeOne;
	private Button UpgradeTwo;
	private Button UpgradeThree;

	 private List<Upgrade> displayedUpgrades = new List<Upgrade>();

	 private Random rand = new Random();

	public override void _Ready()
	{
		base._Ready();
		Visible = false;
		ProcessMode = ProcessModeEnum.Always;
		GetNode<Button>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/Mod2").Disabled = true;
		GetNode<Button>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/Mod3").Disabled = true;

		PickRandomUpgrades();

		UpgradeOne.Text = displayedUpgrades[0].UpgradeName;
		UpgradeTwo.Text = displayedUpgrades[1].UpgradeName;
		UpgradeThree.Text = displayedUpgrades[2].UpgradeName;

		UpgradeOne.Connect("pressed", new Callable(this, nameof(OnUpgradeButtonPressed)));
		UpgradeTwo.Connect("pressed", new Callable(this, nameof(OnUpgradeButtonPressed)));
		UpgradeThree.Connect("pressed", new Callable(this, nameof(OnUpgradeButtonPressed)));
	}
	
	private void PickRandomUpgrades() //randomize displayed upgrades
	{
		displayedUpgrades.Clear();
		while (displayedUpgrades.Count < 3)
		{
			int randomIndex = rand.Next(allUpgrades.Count);
			// Ensure no duplicate upgrades
			if (!displayedUpgrades.Contains(allUpgrades[randomIndex]))
			{
				displayedUpgrades.Add(allUpgrades[randomIndex]);
			}
		}
	}

	 private void OnUpgradeButtonPressed(int upgradeIndex)
	{
		Upgrade selectedUpgrade = displayedUpgrades[upgradeIndex];
		GD.Print("Selected Upgrade: " + selectedUpgrade.UpgradeName);

		// Apply the upgrade logic here
		ApplyUpgrade(selectedUpgrade);
	}

		// Apply the upgrade to the player (this is where your game-specific logic would go)
	private void ApplyUpgrade(Upgrade upgrade)
	{
		// Example: Apply upgrade effects based on the selected upgrade
		switch (upgrade.UpgradeName)
		{
			case "Double Jump":
				// Enable double jump logic
				break;
			case "Speed Boost":
				// Increase player speed
				break;
			case "Health Regen":
				// Start health regeneration
				break;
			// Add more cases for each upgrade type
			default:
				break;
		}
		QueueFree(); 
	}

}
