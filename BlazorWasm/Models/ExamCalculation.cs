namespace BlazorWasm.Models;

public sealed record ExamCalculation(
    DateTime Start,
    TimeSpan StandardDuration,
    decimal ExtraTimePercent,
    TimeSpan AdditionalTime,
    TimeSpan AdjustedExamDuration,
    int BreakMinutes,
    DateTime StandardFinish,
    DateTime AdjustedExamFinish,
    DateTime FinalFinish,
    double StandardShare,
    double ExtraShare,
    double BreakShare)
{
    public decimal Multiplier => 1m + (ExtraTimePercent / 100m);
}
