using System;

namespace Assets.Scripts.SaveSystem
{
    [Serializable]
    public class PlayerData
    {
        public string CurrentLevel { get; set; }
        public double CurrentHealth { get; set; }
        public float CurrentMana { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public PlayerData() { }

        public PlayerData(string currentLevel, double currentHealth, float currentMana, float positionX, float positionY, float positionZ)
        {
            CurrentLevel = currentLevel;
            CurrentHealth = currentHealth;
            CurrentMana = currentMana;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }
    }
}
