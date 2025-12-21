using System.ComponentModel.DataAnnotations;

namespace HeatExchangeCalculator.Models
{
    public class Calculation
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите название расчета")]
        [StringLength(100, ErrorMessage = "Название не более 100 символов")]
        public string Name { get; set; } = "Новый расчет";
        
        
        [Required(ErrorMessage = "Введите высоту слоя")]
        [Range(0.1, 100, ErrorMessage = "Высота должна быть от 0.1 до 100 м")]
        public double LayerHeight { get; set; } = 5.0;           // Высота слоя H₀
        
        [Required(ErrorMessage = "Введите температуру материала")]
        [Range(-273, 2000, ErrorMessage = "Температура должна быть от -273 до 2000°C")]
        public double MaterialInitialTemp { get; set; } = 5.0;   // t′
        
        [Required(ErrorMessage = "Введите температуру газа")]
        [Range(-273, 2000, ErrorMessage = "Температура должна быть от -273 до 2000°C")]
        public double GasInitialTemp { get; set; } = 750.0;      // T′
        
        [Required(ErrorMessage = "Введите скорость газа")]
        [Range(0.01, 10, ErrorMessage = "Скорость должна быть от 0.01 до 10 м/с")]
        public double GasVelocity { get; set; } = 0.72;          // wг
        
        [Required(ErrorMessage = "Введите теплоемкость газа")]
        [Range(0.1, 5, ErrorMessage = "Теплоемкость должна быть от 0.1 до 5 кДж/(м³·К)")]
        public double GasHeatCapacity { get; set; } = 0.9;       // Cг
        
        [Required(ErrorMessage = "Введите расход материала")]
        [Range(0.1, 10, ErrorMessage = "Расход должен быть от 0.1 до 10 кг/с")]
        public double MaterialFlowRate { get; set; } = 1.66;     // Gм
        
        [Required(ErrorMessage = "Введите теплоемкость материала")]
        [Range(0.1, 5, ErrorMessage = "Теплоемкость должна быть от 0.1 до 5 кДж/(кг·К)")]
        public double MaterialHeatCapacity { get; set; } = 1.49; // Cм
        
        [Required(ErrorMessage = "Введите коэффициент теплоотдачи")]
        [Range(100, 10000, ErrorMessage = "Коэффициент должен быть от 100 до 10000 Вт/(м³·К)")]
        public double HeatTransferCoefficient { get; set; } = 2430.0; // αV
        
        [Required(ErrorMessage = "Введите диаметр аппарата")]
        [Range(0.5, 10, ErrorMessage = "Диаметр должен быть от 0.5 до 10 м")]
        public double ApparatusDiameter { get; set; } = 2.3;     // D
        
        // Системные поля
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public Calculation Clone()
        {
            return new Calculation
            {
                Id = this.Id,
                Name = this.Name,
                LayerHeight = this.LayerHeight,
                MaterialInitialTemp = this.MaterialInitialTemp,
                GasInitialTemp = this.GasInitialTemp,
                GasVelocity = this.GasVelocity,
                GasHeatCapacity = this.GasHeatCapacity,
                MaterialFlowRate = this.MaterialFlowRate,
                MaterialHeatCapacity = this.MaterialHeatCapacity,
                HeatTransferCoefficient = this.HeatTransferCoefficient,
                ApparatusDiameter = this.ApparatusDiameter,
                CreatedDate = this.CreatedDate,
                ModifiedDate = this.ModifiedDate
            };
        }
    }
}