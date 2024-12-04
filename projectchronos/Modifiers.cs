using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

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
		new Upgrade("FIRE DAMAGE RESISTANCE", "Resistant to fire damage."),
		new Upgrade("DECREASE ENEMY SPAWN RATE", "Lowers enemy spawns."),
		new Upgrade("INCREASED DEFENSE", "Increases player defense."),
		new Upgrade("DOUBLE JUMP", "Allows player to jump once more mid-air")
	
	};

	 private List<Upgrade> displayedUpgrades = new List<Upgrade>();

	 private Random rand = new Random();

	 private Timer regenTimer;
	 private bool isHealthRegenerating = false;  // Flag to track if regen is active
	 private int regenAmount = 2;  // Amount of health to regenerate every interval
	 private int regenInterval = 2;

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

	public void OnUpgradeOnePressed() {
		ApplyUpgrade(displayedUpgrades[0]);
		Visible = false;
		GetTree().Paused = false;
	}

	public void OnUpgradeTwoPressed() {
		ApplyUpgrade(displayedUpgrades[1]);
		Visible = false;
		GetTree().Paused = false;
	}

	public void OnUpgradeThreePressed() {
		ApplyUpgrade(displayedUpgrades[2]);
		Visible = false;
		GetTree().Paused = false;
	}

		// Apply the upgrade to the player
	private void ApplyUpgrade(Upgrade upgrade)
	{
		switch (upgrade.UpgradeName)
		{
			case "INCREASED HEALTH":
			var player = GetNode<Player>("/root/Main/LevelContainer/TartarusLevel/Player");
				player.PlayerMaxHp += 10;
				break;
			case "SPEED BOOST":
			player = GetNode<Player>("/root/Main/LevelContainer/TartarusLevel/Player");
				player.speed += 100;
				break;
			case "HEALTH REGEN":
				StartHealthRegeneration();
				break;
			case "INCREASED DAMAGE":
				//placeholder
				break;
			case "FIRE DAMAGE RESISTANCE":
			player = GetNode<Player>("/root/Main/LevelContainer/TartarusLevel/Player");
				player.SetFireDuration(0);
				break;
			case "DECREASE ENEMY SPAWN RATE":
			GetNode<EnemySpawner>("/root/Main/LevelContainer/TartarusLevel/EnemySpawner").SpawnTimer.WaitTime = 8;
			
				break;
			case "DOUBLE JUMP":
			// placeholder 
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
		Player player = null;
		Node parent = GetParent();
		for (int i = 0; i < parent.GetChildCount(); i++) {
			if (parent.GetChild(i) is Player p) {
				player = p;
			}
		}
		if (player.PlayerHp < player.PlayerMaxHp)
		{
			player.PlayerHp = Mathf.Min(player.PlayerHp + regenAmount, player.PlayerMaxHp);
			//GD.Print("Health regenerating: " + player.PlayerHp);
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
