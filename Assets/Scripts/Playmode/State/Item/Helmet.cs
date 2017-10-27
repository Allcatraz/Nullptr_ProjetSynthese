namespace ProjetSynthese
{
    public class Helmet : Item
    {
        private const int ProtectionValueLevel1 = 15;
        private const int ProtectionValueLevel2 = 30;
        private const int ProtectionValueLevel3 = 50;

        private static readonly int[] ProtectionValues = { ProtectionValueLevel1, ProtectionValueLevel2, ProtectionValueLevel3 };

        private const int Weight = 0;

        public int ProtectionValue
        {
            get
            {
                return ProtectionValues[Level - 1];
            }
        }

        public override int GetWeight()
        {
            return Weight;
        }
    }
}
