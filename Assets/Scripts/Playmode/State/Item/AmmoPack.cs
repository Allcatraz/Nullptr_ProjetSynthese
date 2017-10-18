namespace ProjetSynthese
{
    public class AmmoPack : Item
    {
        public AmmoType AmmoType { get; set; }
        public int NumberOfAmmo { get; set; }

        private static int weightPer30Bullets = 5;

        public override int GetWeight()
        {
            return weightPer30Bullets * (NumberOfAmmo % 30);
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}

