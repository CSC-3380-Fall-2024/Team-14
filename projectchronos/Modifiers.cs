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
		new Upgrade("INCREASED HEALTH", "Increases your total health."),
		new Upgrade("SPEED BOOST", "Increases your movement speed."),
		new Upgrade("HEALTH REGEN", "Regenerates health over time."),
		new Upgrade("INCREASED DAMAGE", "Increases your attack damage."),
		new Upgrade("FIRE DAMAGE NEGATION", "Immune to fire damage."),
		new Upgrade("DECREASE ENEMY SPAWN RATE", "Lowers enemy spawns.")
		//we can add more later im tired lmao
	
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

		UpgradeOne.Pressed += () => OnUpgradePressed(0);
		UpgradeTwo.Pressed += () => OnUpgradePressed(1);
		UpgradeThree.Pressed += () => OnUpgradePressed(2);

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

	 private void OnUpgradePressed(int upgradeIndex)
	{
		Upgrade selectedUpgrade = displayedUpgrades[upgradeIndex];
		GD.Print("Selected Upgrade: " + selectedUpgrade.UpgradeName);

		ApplyUpgrade(selectedUpgrade);
		
		Visible = false;
		GetTree().Paused = false;
	}

		// Apply the upgrade to the player
	private void ApplyUpgrade(Upgrade upgrade)
	{
		switch (upgrade.UpgradeName)
		{
			case "INCREASED HEALTH":
				Player player = GetNode<Player>("/root/Main/World/Player");
				player.PlayerMaxHp += 20;
				break;
			case "SPEED BOOST":
				Player player1 = GetNode<Player>("/root/Main/World/Player");
				player1.speed += 200;
				break;
			case "HEALTH REGEN":
				
				break;
			case "INCREASED DAMAGE":
				//placeholder
				break;
			case "FIRE DAMAGE NEGATION":
				//placeholder
				break;
			case "DECREASE ENEMY SPAWN RATE":
				//placeholder
				break;
		}
	}

}
