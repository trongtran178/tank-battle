using System;
public class InitBulletTimeData
{
    public InitBulletTimeData(int bulletOrder, float bulletMana)
    {
        BulletOrder = bulletOrder;
        BulletMana = bulletMana;
    }

    public int BulletOrder { get; set; }
    public float BulletMana { get; set; }
}
