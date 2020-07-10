using System;
namespace Assets.Scripts.SaveSystem
{
    [Serializable]
    public class InitBulletTimeData
    {
        public int BulletOrder { get; set; }
        public float BulletMana { get; set; }

        public InitBulletTimeData(int bulletOrder, float bulletMana)
        {
            BulletOrder = bulletOrder;
            BulletMana = bulletMana;
        }
    }
}
