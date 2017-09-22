using UnityEngine;

namespace ProjetSynthese
{
    //[CreateAssetMenu(fileName = "New Effect", menuName = "Game/Effect/Effect")]
    public abstract class Effect : ScriptableObject
    {
        public abstract void ApplyOn(GameScript effectExecutor, GameObject topParent);
    }
}