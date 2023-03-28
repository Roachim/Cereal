using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Web;

namespace CerealAPI.DTO
{
    public class CerealDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mfr { get; set; }
        public string Type { get; set; }
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Fat { get; set; }
        public int Sodium { get; set; }
        public double Fiber { get; set; }
        public double Carbo { get; set; }
        public int Sugars { get; set; }
        public int Potass { get; set; }
        public int Vitamins { get; set; }
        public int Shelf { get; set; }
        public double Weight { get; set; }
        public double Cups { get; set; }
        public double Rating { get; set; }
        public CerealDTO() { }
        //constructor meant for getting existing cereal
        public CerealDTO(int id, string name="0", string mfr = "0", string type = "0", int calories = 0, 
            int protein = 0, int fat = 0, int sodium = 0, double fiber = 0, double carbo = 0, 
            int sugars = 0, int potass = 0, int vitamins = 0, int shelf = 0, 
            double weight = 0, double cups = 0, double rating = 0) 
        {
            Id = id;
            Name = name;
            Mfr = mfr;
            Type = type;
            Calories = calories;
            Protein = protein;
            Fat = fat;
            Sodium = sodium;
            Fiber = fiber;
            Carbo = carbo;
            Sugars = sugars;
            Potass = potass;
            Vitamins = vitamins;
            Shelf =  shelf;
            Weight = weight;
            Cups = cups;
            Rating = rating;
        }
        //Constructor meant for creating new cereal
        public CerealDTO(string name = "0", string mfr = "0", string type = "0", int calories = 0, int protein = 0, int fat = 0,
            int sodium = 0, float fiber = 0, float carbo = 0,
            int sugars = 0, int potass = 0, int vitamins = 0, int shelf = 0, float weight = 0, float cups = 0, 
            float rating = 0)
        {
            Name = name;
            Mfr = mfr;
            Type = type;
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
