using UnityEngine;
using System;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int experienceToNextLevel = 5;

    public event Action<int> LeveledUp;

    public void AddExperience(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentExperience += amount;
        Debug.Log($"XP: {currentExperience}/{experienceToNextLevel}");

        while (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level += 1;
        experienceToNextLevel += 3;

        Debug.Log($"Level Up! Level: {level}");
        LeveledUp?.Invoke(level);
    }
}
