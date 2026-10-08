using System.Globalization;
using BlazorWasm.Models;
using BlazorWasm.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWasm.Pages;

public partial class Index
{
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private DateTime ExamDate { get; set; } = DateTime.Today;
    private TimeOnly StartTime { get; set; } = new(9, 0);

    private int examDurationMinutes = 60;
    private decimal extraTimePercent;
    private int breakMinutes;

    private int ExamDurationMinutes
    {
        get => examDurationMinutes;
        set => examDurationMinutes = Math.Clamp(value, 1, 1440);
    }

    private decimal ExtraTimePercent
    {
        get => extraTimePercent;
        set => extraTimePercent = Math.Clamp(value, 0m, 200m);
    }

    private int BreakMinutes
    {
        get => breakMinutes;
        set => breakMinutes = Math.Clamp(value, 0, 240);
    }

    private bool copied;

    private static readonly TimePreset[] ExtraTimePresets =
    [
        new("Standard", 0m),
        new("1.25×", 25m),
        new("1.5×", 50m),
        new("1.75×", 75m),
        new("2×", 100m),
    ];

    private ExamCalculation Calculation =>
        ExamTimeCalculator.Calculate(
            ExamDate,
            StartTime.ToTimeSpan(),
            ExamDurationMinutes,
            ExtraTimePercent,
            BreakMinutes);

    private void SetDuration(int minutes)
    {
        ExamDurationMinutes = Math.Clamp(minutes, 1, 1440);
    }

    private void SetExtraTime(decimal percent)
    {
        ExtraTimePercent = Math.Clamp(percent, 0m, 200m);
    }

    private void Reset()
    {
        ExamDate = DateTime.Today;
        StartTime = new TimeOnly(9, 0);
        ExamDurationMinutes = 60;
        ExtraTimePercent = 0;
        BreakMinutes = 0;
        copied = false;
    }

    private string GetActivePresetClass(decimal percent) =>
        ExtraTimePercent == percent
            ? "preset-button is-active"
            : "preset-button";

    private static string FormatTime(DateTime value) =>
        value.ToString("h:mm tt", CultureInfo.CurrentCulture);

    private string FormatFinish(DateTime value)
    {
        var time = FormatTime(value);

        return value.Date == ExamDate.Date
            ? time
            : value.ToString("ddd, MMM d • h:mm tt", CultureInfo.CurrentCulture);
    }

    private static string FormatDuration(TimeSpan value)
    {
        var hours = (int)value.TotalHours;
        var minutes = value.Minutes;

        return hours > 0
            ? minutes > 0
                ? $"{hours}h {minutes}m"
                : $"{hours}h"
            : $"{minutes}m";
    }

    private static string FormatMinutes(int minutes)
    {
        if (minutes < 60)
        {
            return $"{minutes} min";
        }

        var hours = minutes / 60;
        var remainder = minutes % 60;

        return remainder == 0
            ? $"{hours} hr"
            : $"{hours} hr {remainder} min";
    }

    private string BuildCopyText()
    {
        var result = Calculation;

        return $"""
            Exam Time Calculator
            Date: {ExamDate.ToString("dddd, MMMM d, yyyy", CultureInfo.CurrentCulture)}
            Start: {FormatTime(result.Start)}
            Standard exam: {FormatDuration(result.StandardDuration)} → {FormatFinish(result.StandardFinish)}
            Extra time: {result.ExtraTimePercent:0.#}% (+{FormatDuration(result.AdditionalTime)})
            Adjusted exam: {FormatDuration(result.AdjustedExamDuration)} → {FormatFinish(result.AdjustedExamFinish)}
            Break / additional time: {result.BreakMinutes} min
            Final finish: {FormatFinish(result.FinalFinish)}
            """;
    }

    private async Task CopyResult()
    {
        try
        {
            await JS.InvokeVoidAsync("examCalc.copyText", BuildCopyText());
            copied = true;
            await InvokeAsync(StateHasChanged);

            await Task.Delay(1400);
        }
        catch (JSException)
        {
            copied = false;
        }

        copied = false;
        await InvokeAsync(StateHasChanged);
    }

    private readonly record struct TimePreset(string Label, decimal Percent);
}
