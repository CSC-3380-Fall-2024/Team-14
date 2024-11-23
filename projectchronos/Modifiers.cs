using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

public partial class Modifiers : Control
{
private Player player;
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
		new Upgrade("FIRE DAMAGE RESISTANCE", "Immune to fire damage."),
		new Upgrade("DECREASE ENEMY SPAWN RATE", "Lowers enemy spawns."),
		new Upgrade("INCREASED DEFENSE", "Increases player defense."),
		new Upgrade("DOUBLE JUMP", "Allows player to jump once more mid-air")
	
	};

	 private List<Upgrade> displayedUpgrades = new List<Upgrade>();

	 private Random rand = new Random();

	 private Timer regenTimer;
	 private bool isHealthRegenerating = false;  // Flag to track if regen is active
	 private int regenAmount = 5;  // Amount of health to regenerate every interval
	 private int regenInterval = 5;

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

		regenTimer = new Timer();
		regenTimer.WaitTime = regenInterval;
		regenTimer.OneShot = false;
		regenTimer.Autostart = false;
		regenTimer.Connect("timeout", new Callable(this, "OnHealthRegenerate"));
		AddChild(regenTimer);

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
		// Example: Apply upgrade effects based on the selected upgrade
		switch (upgrade.UpgradeName)
		{
			case "INCREASED HEALTH":
				player.PlayerMaxHp += 10;
				break;
			case "SPEED BOOST":
				player.speed += 100;
				break;
			case "HEALTH REGEN":
				StartHealthRegeneration();
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
			case "DOUBLE JUMP":

				break;
		}
	}

	 // Start the health regeneration process
	private void StartHealthRegeneration()
	{
		if (!isHealthRegenerating)
		{
			isHealthRegenerating = true;
			regenTimer.Start();
		}
	}

	 private void OnHealthRegenerate()
	{
		if (player.PlayerHp < player.PlayerMaxHp)
		{
			player.PlayerHp = Mathf.Min(player.PlayerHp + regenAmount, player.PlayerMaxHp);
			GD.Print("Health regenerating: " + player.PlayerHp);
		}
	}

	 public void StopHealthRegeneration()
	{
		if (isHealthRegenerating)
		{
			isHealthRegenerating = false;
			regenTimer.Stop();
		}
	}

}
