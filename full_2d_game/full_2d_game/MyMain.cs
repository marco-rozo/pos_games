using Godot;
using System;

public partial class MyMain : Node2D
{

    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;

    public void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();

        GetNode<MyHUD>("HUD").ShowGameOver();
    }

    public void NewGame()
    {
        _score = 0;

        var player = GetNode<MyPlayer>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();

        var hud = GetNode<MyHUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");
    }

    private void OnScoreTimerTimeout()
    {
        _score++;

        GetNode<MyHUD>("HUD").UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        GD.Print("OnStartTimerTimeout");
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnMobTimerTimeout()
    {
        MyMob mob = MobScene.Instantiate<MyMob>();

        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        mob.Position = mobSpawnLocation.Position;

        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        AddChild(mob);
    }


}
