namespace SmartDailyTools.Services;

public record EmiResult(double Emi, double TotalInterest, double TotalPayment);

public interface IEmiService
{
    EmiResult Calculate(double principal, double annualRate, int tenureMonths);
}

public class EmiService : IEmiService
{
    public EmiResult Calculate(double principal, double annualRate, int tenureMonths)
    {
        if (principal <= 0 || tenureMonths <= 0) return new EmiResult(0, 0, 0);

        double monthlyRate = annualRate / 12.0 / 100.0;

        double emi;
        if (monthlyRate == 0)
        {
            emi = principal / tenureMonths;
        }
        else
        {
            double pow = Math.Pow(1 + monthlyRate, tenureMonths);
            emi = principal * monthlyRate * pow / (pow - 1);
        }

        double totalPayment = emi * tenureMonths;
        double totalInterest = totalPayment - principal;

        return new EmiResult(Math.Round(emi, 2), Math.Round(totalInterest, 2), Math.Round(totalPayment, 2));
    }
}
