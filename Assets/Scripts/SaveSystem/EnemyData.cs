using System;
using Assets.Scripts.Enemy;

namespace Assets.Scripts.SaveSystem
{
    [System.Serializable]
    public class EnemyData
    {
        private EnemyType type;
        private float currentHeath;
        private float positionX;
        private float positionY;
        private float positionZ;

        public EnemyData(EnemyType type, float currentHeath, float positionX, float positionY, float positionZ)
        {
            Type = type;
            CurrentHeath = currentHeath;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public EnemyType Type { get => type; set => type = value; }
        public float CurrentHeath { get => currentHeath; set => currentHeath = value; }
        public float PositionX { get => positionX; set => positionX = value; }
        public float PositionY { get => positionY; set => positionY = value; }
        public float PositionZ { get => positionZ; set => positionZ = value; }
    }
}
