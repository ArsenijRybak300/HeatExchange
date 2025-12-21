namespace HeatExchangeCalculator.Models
{
    public class CalculationResult
    {
        public Calculation? OriginalCalculation { get; set; } // Добавьте ?
        public List<PointResult> Points { get; set; } = new(); // Инициализируйте
        public double ParameterM { get; set; }
        public double Y0 { get; set; }
        public double Denominator { get; set; }
        public string? ErrorMessage { get; set; } // Добавьте ?
        
        public class PointResult
        {
            public double Height { get; set; }
            public double RelativeHeight { get; set; }
            public double MaterialTemperature { get; set; }
            public double GasTemperature { get; set; }
            public double TemperatureDifference { get; set; }
        }
    }
}