using UnityEngine;
namespace Assets.Scripts.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public abstract void UpgrageLevelCorrespondToPhase(Phase phase);
        public abstract void Instantiate();
    }
}
