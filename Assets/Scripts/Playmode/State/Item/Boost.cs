namespace ProjetSynthese
{
    public class Boost : Usable
    {
        public const int Level1Efficacity = 5;
        public const int Level2Efficacity = 15;
        public const int Level3Efficacity = 35;

        private static readonly int[] Efficacities = { Level1Efficacity, Level2Efficacity, Level3Efficacity };

        private const int Level1Weight = 1;
        private const int Level2Weight = 10;
        private const int Level3Weight = 15;

        private static readonly int[] Weights = { Level1Weight, Level2Weight, Level3Weight };

        public int Efficacity
        {
            get
            {
                return Efficacities[Level - 1];
            }
        }

        public override void Use()
        {
            Player.GetComponentInChildren<BoostStats>().Heal(Efficacity);
            //Todo: Enlever un boost dans l'inventaire
        }

        public override int GetWeight()
        {
            return Weights[Level - 1];
        }
    }
}
