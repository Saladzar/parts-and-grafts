using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node
{
    [Export] public Marker2D SpawnPoint;
    [Export] private PackedScene EnemyScene;
    int maxEnemies = 10;
	
    float timer = 0f;
    float spawnRate = 5f;
    Random rnd = new Random();
    float spawnRadius = 500f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (EnemyScene == null) return;
        if (SpawnPoint == null ) return;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        timer += (float)delta;
		
        if(timer>spawnRate)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }
    private Vector2 getRandomSpawnPosition()
    {
        float offsetX = rnd.Next(0, 1080); 
        float offsetY = rnd.Next(0, 720);
        return SpawnPoint.GlobalPosition + new Vector2(offsetX, offsetY);
    }
    private void SpawnEnemy()
    {
        if (SpawnPoint == null || EnemyScene == null)
            return;

        Node2D enemy = EnemyScene.Instantiate<Node2D>();

        Vector2 spawnPosition = getRandomSpawnPosition();

        GetTree().CurrentScene.AddChild(enemy);
        enemy.GlobalPosition = spawnPosition;

        enemy.AddToGroup("enemies");

        GD.Print($"Enemy spawned at: {spawnPosition}");
    }
}