using Godot;
using System;

public partial class World : Node2D
{

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Show();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Show();
	}

	//grabs value for Max_Lives
	

	public void Start(Vector2 position)
	{
		//Position = position;
		Show();
		//GetNode<TextureRect>("TextureRect");
	}


}
