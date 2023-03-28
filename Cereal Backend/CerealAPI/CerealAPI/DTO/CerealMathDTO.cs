namespace CerealAPI.DTO
{
    public class CerealMathDTO
    {
        public string Name { get; set; }
        public string Mfr { get; set; }
        public string Type { get; set; }
        public string Calories { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Sodium { get; set; }
        public string Fiber { get; set; }
        public string Carbo { get; set; }
        public string Sugars { get; set; }
        public string Potass { get; set; }
        public string Vitamins { get; set; }
        public string Shelf { get; set; }
        public string Weight { get; set; }
        public string Cups { get; set; }
        public string Rating { get; set; }
        public CerealMathDTO() { }
        //constructor meant for getting existing cereal
        public CerealMathDTO(string calories = "0", string protein = "0", string fat = "0", string sodium = "0", 
            string fiber = "0", string carbo = "0", string sugars = "0", string potass = "0", string vitamins = "0", 
            string shelf = "0", string weight = "0", string cups = "0", string rating = "0")
        {
            Calories = calories;
            Protein = protein;
            Fat = fat;
            Sodium = sodium;
            Fiber = fiber;
            Carbo = carbo;
            Sugars = sugars;
            Potass = potass;
            Vitamins = vitamins;
            Shelf = shelf;
            Weight = weight;
            Cups = cups;
            Rating = rating;
        }
    }
}
