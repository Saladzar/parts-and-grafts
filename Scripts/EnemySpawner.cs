using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene EnemyScene;

    [Export] private AnimalsData[] AnimalsDataArray;

    [Export] private float MinEnemySize = 0.5f;
    [Export] private float BaseMaxEnemySize = 2.0f;
    [Export] private int minScore = 0, maxScore = 5;
    [Export] private float BaseSpawnRate = 5f;
    [Export] private float MaxEnemySizeIncreasePerMinute = 0.5f;
    [Export] private float SpawnRateDecreasePerMinute = 0.5f;
    [Export] private float MinSpawnRate = 1.5f;

    int maxEnemies = 10;
	
    float timer = 0f;
    Random rnd = new Random();
    float spawnRadius = 500f;

    private Vector2 playerPosition;
    private GameManager gameManager;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (EnemyScene == null) return;
        gameManager = GetNodeOrNull<GameManager>("../GameManager");
        if (gameManager == null)
            GD.PushWarning("EnemySpawner could not find GameManager.");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        timer += (float)delta;
        playerPosition = GetNodeOrNull<Player>("../Player")?.GetPosition() ?? Vector2.Zero;
		
        float currentSpawnRate = GetCurrentSpawnRate();
        if(timer > currentSpawnRate)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    private float GetCurrentSpawnRate()
    {
        if (gameManager == null)
            return BaseSpawnRate;

        int gameTime = gameManager.GetGameTime();
        float spawnRateReduction = (gameTime / 60f) * SpawnRateDecreasePerMinute;
        return Mathf.Max(BaseSpawnRate - spawnRateReduction, MinSpawnRate);
    }

    private float GetCurrentMaxEnemySize()
    {
        if (gameManager == null)
            return BaseMaxEnemySize;

        int gameTime = gameManager.GetGameTime();
        float sizeIncrease = (gameTime / 60f) * MaxEnemySizeIncreasePerMinute;
        return BaseMaxEnemySize + sizeIncrease;
    }
    private Vector2 getRandomSpawnPosition()
    {
        float offsetX = rnd.Next(-960, 960); 
        float offsetY = rnd.Next(-540, 540);
        return playerPosition + new Vector2(offsetX, offsetY);
    }
    private void SpawnEnemy()
    {
        if (EnemyScene == null || AnimalsDataArray == null || AnimalsDataArray.Length == 0)
        {
            GD.PrintErr("Cannot spawn enemy: EnemyScene or AnimalsDataArray is not configured.");
            return;
        }

        Enemy enemy = EnemyScene.Instantiate<Enemy>();
        Vector2 spawnPosition = getRandomSpawnPosition();

        GetTree().CurrentScene.AddChild(enemy);
        enemy.GlobalPosition = spawnPosition;
        
        AnimalsData animalData = AnimalsDataArray[rnd.Next(AnimalsDataArray.Length)];
        if (animalData != null)
            enemy.MakeType(animalData);
        else
            GD.PrintErr("AnimalsData at index is null.");
        
        float currentMaxSize = GetCurrentMaxEnemySize();
        float randomSize = (float)rnd.NextDouble() * (currentMaxSize - MinEnemySize) + MinEnemySize;
        int randomScore = (int)Math.Round(rnd.NextDouble() * maxScore);
        enemy.SetSize(randomSize);
        enemy.SetScore(randomScore);
        enemy.ConfigureLabel();

        enemy.AddToGroup("enemies");

        GD.Print($"Enemy spawned at: {spawnPosition}");
    }
}