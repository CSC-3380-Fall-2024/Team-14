namespace ProjectChronos;

public partial class Sisyphus: BasicEnemy, BasicEnemy.EnemyAI {
	public override void _Ready()
	{
		MaxLife = 1000;
		CurrentLife = 1000;
		ai = this;
		base._Ready();
	}

	public float DistanceToRoll = 400f;
	public float SecondsToRoll = 1f;

	private float lastRolled;
	
	public void ExecuteAI(float delta)
	{
		if (DistanceToPlayer() < DistanceToRoll)
		{
			lastRolled += delta;
			if (lastRolled > SecondsToRoll)
			{
				lastRolled -= SecondsToRoll;
				GetParent().AddChild(Rollingrock.CreateRock(Position, PlayerPosition()));
			}
		}
	}
}
