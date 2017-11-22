namespace ProjetSynthese
{
    public class Heal : Usable
    {
        private const int Level1Efficacity = 5;
        private const int Level2Efficacity = 75;
        private const int Level3Efficacity = 100;

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

        public override bool Use()
        {
            Player.GetComponentInChildren<Health>().Heal(Efficacity);
            return true;
        }

        public override int GetWeight()
        {
            return Weights[Level - 1];
        }
    }
}
