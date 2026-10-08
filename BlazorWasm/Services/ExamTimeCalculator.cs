using BlazorWasm.Models;

namespace BlazorWasm.Services;

public static class ExamTimeCalculator
{
    public static ExamCalculation Calculate(
        DateTime examDate,
        TimeSpan startTime,
        int examMinutes,
        decimal extraTimePercent,
        int breakMinutes)
    {
        examMinutes = Math.Clamp(examMinutes, 1, 24 * 60);
        extraTimePercent = Math.Clamp(extraTimePercent, 0m, 200m);
        breakMinutes = Math.Clamp(breakMinutes, 0, 240);

        var start = examDate.Date.Add(startTime);

        var standardFinish = start.AddMinutes(examMinutes);

        var adjustedMinutesDecimal =
            examMinutes * (1m + (extraTimePercent / 100m));

        var adjustedMinutes = Math.Max(
            examMinutes,
            (int)Math.Ceiling(adjustedMinutesDecimal));

        var additionalMinutes = adjustedMinutes - examMinutes;
        var adjustedExamFinish = start.AddMinutes(adjustedMinutes);
        var finalFinish = adjustedExamFinish.AddMinutes(breakMinutes);

        var totalMinutes = Math.Max(
            1,
            (finalFinish - start).TotalMinutes);

        return new ExamCalculation(
            Start: start,
            StandardDuration: TimeSpan.FromMinutes(examMinutes),
            ExtraTimePercent: extraTimePercent,
            AdditionalTime: TimeSpan.FromMinutes(additionalMinutes),
            AdjustedExamDuration: TimeSpan.FromMinutes(adjustedMinutes),
            BreakMinutes: breakMinutes,
            StandardFinish: standardFinish,
            AdjustedExamFinish: adjustedExamFinish,
            FinalFinish: finalFinish,
            StandardShare: (examMinutes / totalMinutes) * 100d,
            ExtraShare: (additionalMinutes / totalMinutes) * 100d,
            BreakShare: (breakMinutes / totalMinutes) * 100d);
    }
}
