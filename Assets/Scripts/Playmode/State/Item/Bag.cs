namespace ProjetSynthese
{
    public class Bag : Item
    {      
        private const int Level1Capacity = 100;
        private const int Level2Capacity = 175;
        private const int Level3Capacity = 300;

        private static readonly int[] Capacities = { Level1Capacity , Level2Capacity , Level3Capacity };

        private const int Weight = 0;

        public int Capacity
        {
            get
            {
                return Capacities[Level - 1];
            }
        }

        public override int GetWeight()
        {
            return Weight;
        }
    }
}

