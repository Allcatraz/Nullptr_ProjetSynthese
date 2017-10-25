namespace ProjetSynthese
{
    public class AmmoPack : Item
    {
        private const int WeightPer30Bullets = 5;

        public AmmoType AmmoType { get; set; }
        public int NumberOfAmmo { get; set; }

        public override int GetWeight()
        {
            return WeightPer30Bullets * (NumberOfAmmo % 30);
        }
    }
}

