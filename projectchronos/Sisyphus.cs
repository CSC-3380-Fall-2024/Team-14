namespace ProjectChronos;

public partial class Sisyphus: BasicEnemy {
    public override void _Ready()
    {
        MaxLife = 1000;
        CurrentLife = 1000;
        base._Ready();
    }
}