using Godot;
using System;
using System.Numerics;

public partial class Main : Node {

	[Export]
	public PackedScene GameWorldScene {get; set;}

	private TextureRect exitScreen; 

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

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

//shows the exit screen and basically just unhides it after death
	public void ShowExit()
	{
		exitScreen.Visible = true;
	}
}
