﻿using System;
namespace Assets.Scripts.SaveSystem
{
    [System.Serializable]
    public class AlliesObjectData
    {
        private AlliesType type; // Tank, Plane, Dog
        private float currentHeath;
        private float positionX;
        private float positionY;
        private float positionZ;

        public AlliesObjectData() { }

        public AlliesObjectData(AlliesType type, float currentHeath, float positionX, float positionY, float positionZ)
        {
            Type = type;
            CurrentHeath = currentHeath;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public AlliesType Type { get => type; set => type = value; }
        public float CurrentHeath { get => currentHeath; set => currentHeath = value; }
        public float PositionX { get => positionX; set => positionX = value; }
        public float PositionY { get => positionY; set => positionY = value; }
        public float PositionZ { get => positionZ; set => positionZ = value; }
    }
}