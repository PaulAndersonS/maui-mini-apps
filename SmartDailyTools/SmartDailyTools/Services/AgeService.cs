namespace SmartDailyTools.Services;

public record AgeResult(int Years, int Months, int Days, int TotalDays);

public interface IAgeService
{
    AgeResult Calculate(DateTime dateOfBirth, DateTime? referenceDate = null);
}

public class AgeService : IAgeService
{
    public AgeResult Calculate(DateTime dateOfBirth, DateTime? referenceDate = null)
    {
        var today = (referenceDate ?? DateTime.Today).Date;
        var dob = dateOfBirth.Date;

        if (dob > today) return new AgeResult(0, 0, 0, 0);

        int years = today.Year - dob.Year;
        int months = today.Month - dob.Month;
        int days = today.Day - dob.Day;

        if (days < 0)
        {
            months--;
            var lastMonth = today.AddMonths(-1);
            days += DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
        }

        if (months < 0)
        {
            years--;
            months += 12;
        }

        int totalDays = (int)(today - dob).TotalDays;
        return new AgeResult(years, months, days, totalDays);
    }
}
