using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Singleton/MasterManager")]
public class MasterManagerPhoton : SingltonTable<MasterManagerPhoton>
{
    [SerializeField]
    private GameSettingPhoton _gameSettingPhoton;
    public static GameSettingPhoton GameSettingPhoton {
        get
        {
            return Instance._gameSettingPhoton;
        }
    }
}
