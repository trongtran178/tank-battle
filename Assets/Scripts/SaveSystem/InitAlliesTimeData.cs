using System;

namespace Assets.Scripts.SaveSystem
{
    [Serializable]
    public class InitAlliesTimeData
    {
        public AlliesType Type { get; set; }
        public float ManaArmy { get; set; }

        public InitAlliesTimeData(AlliesType type, float manaArmy)
        {
            Type = type;
            ManaArmy = manaArmy;
        }  
    }
}
