using System.Globalization;

namespace CryptoBot.Models
{
    public class MinerInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string HashRate { get; set; }
        public string Power { get; set; }
        public string Cost { get; set; }
        public double HardwareCost { get; set; }
        public override string ToString()
        {
            var hardwareCost = HardwareCost.ToString(CultureInfo.InvariantCulture).Replace(',','.');
            return $"https://whattomine.com/coins/{Id}.json?&hr={HashRate}&p={Power}&fee=0.0&cost=0.06&hcost={hardwareCost}&commit=Calculate";
                
        }
    }

}