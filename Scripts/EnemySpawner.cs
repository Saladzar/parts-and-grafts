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
    [Export] private float ScoreScalingPerMinute = 2.5f;

    private const int MaxEnemies = 8;
	
    float timer = 0f;
    Random rnd = new Random();
    private const float SpawnRadiusMin = 250f;
    private const float SpawnRadiusMax = 700f;
    private bool zeroScoreEnemySpawned = false;

    private Vector2 playerPosition;
    private GameManager gameManager;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (EnemyScene == null) return;

        if (BaseSpawnRate <= 0f) BaseSpawnRate = 5f;
        if (MinSpawnRate <= 0f) MinSpawnRate = 1.5f;
        if (BaseMaxEnemySize <= 0f) BaseMaxEnemySize = 2.0f;
        if (MinEnemySize <= 0f) MinEnemySize = 0.5f;
        if (maxScore <= 0) maxScore = 5;

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
        if (currentSpawnRate <= 0f)
            currentSpawnRate = 1f;

        if (timer >= currentSpawnRate)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    private float GetCurrentSpawnRate()
    {
        float baseRate = BaseSpawnRate > 0f ? BaseSpawnRate : 5f;
        float minRate = MinSpawnRate > 0f ? MinSpawnRate : 1.5f;

        if (gameManager == null)
            return baseRate;

        int gameTime = gameManager.GetGameTime();
        float spawnRateReduction = (gameTime / 60f) * SpawnRateDecreasePerMinute;
        return Mathf.Max(baseRate - spawnRateReduction, minRate);
    }

    private float GetCurrentMaxEnemySize()
    {
        float baseMaxSize = BaseMaxEnemySize > 0f ? BaseMaxEnemySize : 2.0f;

        if (gameManager == null)
            return baseMaxSize;

        int gameTime = gameManager.GetGameTime();
        float sizeIncrease = (gameTime / 60f) * MaxEnemySizeIncreasePerMinute;
        return baseMaxSize + sizeIncrease;
    }

    private int GetCurrentMaxScore()
    {
        int gameTime = gameManager != null ? gameManager.GetGameTime() : 0;
        int currentMaxScore = maxScore > 0 ? maxScore : 5;
        int scoreIncrease = (int)Math.Floor((gameTime / 60f) * ScoreScalingPerMinute);
        return Math.Max(currentMaxScore, minScore + 3 + scoreIncrease);
    }

    private Vector2 getRandomSpawnPosition()
    {
        float angle = (float)rnd.NextDouble() * Mathf.Tau;
        float distance = (float)rnd.NextDouble() * (SpawnRadiusMax - SpawnRadiusMin) + SpawnRadiusMin;
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        return playerPosition + offset;
    }
    private void SpawnEnemy()
    {
        if (EnemyScene == null || AnimalsDataArray == null || AnimalsDataArray.Length == 0)
        {
            GD.PrintErr("Cannot spawn enemy: EnemyScene or AnimalsDataArray is not configured.");
            return;
        }

        if (GetTree() != null && GetTree().GetNodesInGroup("enemies").Count >= MaxEnemies)
            return;

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

        int randomScore;
        int currentGameTime = gameManager != null ? gameManager.GetGameTime() : 0;
        if (currentGameTime < 30 && !zeroScoreEnemySpawned)
        {
            randomScore = 0;
            zeroScoreEnemySpawned = true;
        }
        else
        {
            int currentMaxScore = GetCurrentMaxScore();
            randomScore = rnd.Next(minScore, currentMaxScore + 1);
        }

        enemy.SetSize(randomSize);
        enemy.SetScore(randomScore);
        enemy.ConfigureLabel();

        enemy.AddToGroup("enemies");

        GD.Print($"Enemy spawned at: {spawnPosition}");
    }
}