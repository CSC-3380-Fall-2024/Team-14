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

	 private List<Upgrade> displayedUpgrades = new List<Upgrade>();

	 private Random rand = new Random();

	public override void _Ready()
	{

		Button UpgradeOne = GetNode<UpgradeOne>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/UpgradeOne");
		Button UpgradeTwo = GetNode<UpgradeTwo>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/UpgradeTwo");
		Button UpgradeThree = GetNode<UpgradeThree>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/UpgradeThree");
		
		base._Ready();
		Visible = false;
		ProcessMode = ProcessModeEnum.Always;

		PickRandomUpgrades();

		UpgradeOne.Text = displayedUpgrades[0].UpgradeName;
		UpgradeTwo.Text = displayedUpgrades[1].UpgradeName;
		UpgradeThree.Text = displayedUpgrades[2].UpgradeName;

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

	 private void OnUpgradeOnePressed(int upgradeIndex)
	{
		Upgrade selectedUpgrade = displayedUpgrades[upgradeIndex];
		GD.Print("Selected Upgrade: " + selectedUpgrade.UpgradeName);

		ApplyUpgrade(selectedUpgrade);
		
		Visible = false;
	}

	 private void OnUpgradeTwoPressed(int upgradeIndex)
	{
		Upgrade selectedUpgrade = displayedUpgrades[upgradeIndex];
		GD.Print("Selected Upgrade: " + selectedUpgrade.UpgradeName);

		ApplyUpgrade(selectedUpgrade);

		Visible = false;
	}

	 private void OnUpgradeThreePressed(int upgradeIndex)
	{
		Upgrade selectedUpgrade = displayedUpgrades[upgradeIndex];
		GD.Print("Selected Upgrade: " + selectedUpgrade.UpgradeName);

		ApplyUpgrade(selectedUpgrade);

		Visible = false;
	}

		// Apply the upgrade to the player
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
	}

}
