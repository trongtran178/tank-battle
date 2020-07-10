using System;

namespace Assets.Scripts.SaveSystem
{
    [Serializable]
    public class PlayerData
    {
        public float CurrentHealth { get; set; }
        public float CurrentMana { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public PlayerData() { }

        public PlayerData(float currentHealth, float currentMana, float positionX, float positionY, float positionZ)
        {
            CurrentHealth = currentHealth;
            CurrentMana = currentMana;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }
    }
}
