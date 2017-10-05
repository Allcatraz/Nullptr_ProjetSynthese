using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "DeathCircle", menuName = "Game/Effect/DeathCircle")]
    public class DeathCircle : ScriptableObject
    {
        [Header("Phase 1")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase1;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase1;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase1;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase1;

        [Header("Phase 2")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase2;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase2;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase2;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase2;

        [Header("Phase 3")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase3;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase3;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase3;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase3;

        [Header("Phase 4")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase4;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase4;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase4;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase4;

        [Header("Phase 5")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase5;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase5;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase5;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase5;

        [Header("Phase 6")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase6;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase6;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase6;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase6;

        [Header("Phase 7")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase7;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase7;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase7;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase7;

        [Header("Phase 8")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int waitTimeInSecondPhase8;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int moveTimeInSecondPhase8;

        [LabelOverride("DPS")]
        [SerializeField]
        private int domagePerSecondPhase8;

        [LabelOverride("Skrink")]
        [Range(0, 1)]
        [SerializeField]
        private float shrinkPhase8;
    }
}
