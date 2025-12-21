using HeatExchangeCalculator.Models;

namespace HeatExchangeCalculator.Services
{
    public class HeatExchangeService
    {
        public CalculationResult Calculate(Calculation model)
        {
            var result = new CalculationResult { 
                OriginalCalculation = model.Clone() 
            };
            
            // 1. Площадь сечения аппарата
            double S = Math.PI * Math.Pow(model.ApparatusDiameter / 2, 2);
            
            // 2. Объемный расход газа
            double V_gas = model.GasVelocity * S;
            
            // 3. Теплоемкости потоков, кВт/К
            double W_material = model.MaterialFlowRate * model.MaterialHeatCapacity; // кВт/К
            double W_gas = V_gas * model.GasHeatCapacity; // кВт/К
            
            // 4. Отношение теплоемкостей
            double m = W_material / W_gas;
            result.ParameterM = Math.Round(m, 3);
            
            // 5. Безразмерная высота
            double Y0 = (model.HeatTransferCoefficient * model.LayerHeight) / 
                       (model.GasVelocity * model.GasHeatCapacity * 1000);
            result.Y0 = Math.Round(Y0, 2);
            
            double denominator = 1 - m * Math.Exp((m - 1) * Y0 / m);
            result.Denominator = Math.Round(denominator, 3);
            
            // Проверка на некорректные значения
            if (Math.Abs(denominator) < 0.0001)
            {
                throw new InvalidOperationException("Знаменатель близок к нулю, расчет невозможен. Проверьте входные данные.");
            }
            
            // 7. Расчет для 11 точек
            for (int i = 0; i <= 10; i++)
            {
                // Текущая высота
                double y = model.LayerHeight * i / 10.0;
                
                // Безразмерная высота
                double Y = Y0 * i / 10.0;
                
                // Вспомогательная экспонента
                double expTerm = Math.Exp((m - 1) * Y / m);
                
                // Безразмерные температуры
                double v = (1 - expTerm) / denominator;
                
                double theta = (1 - m * expTerm) / denominator;
                
                // Разность начальных температур
                double deltaT0 = model.GasInitialTemp - model.MaterialInitialTemp;
                
                // Реальные температуры
                double t = model.MaterialInitialTemp + deltaT0 * v;     // Температура материала
                double T = model.MaterialInitialTemp + deltaT0 * theta; // Температура газа
                
                // Проверка на физическую корректность
                if (t < -273 || T < -273)
                {
                    throw new InvalidOperationException("Рассчитана температура ниже абсолютного нуля. Проверьте входные данные.");
                }
                
                result.Points.Add(new CalculationResult.PointResult
                {
                    Height = Math.Round(y, 1),
                    RelativeHeight = Math.Round(Y, 4),
                    MaterialTemperature = Math.Round(t, 1),
                    GasTemperature = Math.Round(T, 1),
                    TemperatureDifference = Math.Round(T - t, 1)
                });
            }
            
            return result;
        }
    }
}