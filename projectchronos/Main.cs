using Godot;
using System;
using System.Numerics;

public partial class Main : Node {

	[Export]
	public PackedScene GameWorldScene {get; set;}

	private TextureRect exitScreen; 

	//sets max number of lives
	public int Max_Lives {get; private set;} = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		//initailize exit screen but hide it so player doesnt't just see DIED lol
		exitScreen = new TextureRect();
		exitScreen.Texture = GD.Load<Texture2D>("res://exit.png");
		//makes hidden
		exitScreen.Visible = false;

		//set size and position
		exitScreen.AnchorTop = 0;
		exitScreen.AnchorBottom = 1;
		exitScreen.AnchorLeft=0;
		exitScreen.AnchorRight=1;

		exitScreen.StretchMode = TextureRect.StretchModeEnum.Scale;

		AddChild(exitScreen);

		//Call start on player
		Player player = GetNode<Player>("World/Player");
		player.Start(new Godot.Vector2 (0,0));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	public int GetMax()
	{
		return Max_Lives;
	}

//shows the exit screen and basically just unhides it after death
	public void ShowExit()
	{
		exitScreen.Visible = true;
	}
}
