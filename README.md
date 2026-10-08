# ExamTime Calculator

A focused, single-page Blazor WebAssembly application for calculating exam finish times with extra-time accommodations.

## What it does

Enter an exam date, start time, standard exam length, and extra-time accommodation. The calculator immediately shows:

- Standard finish time
- Additional accommodation time
- Adjusted exam duration
- Adjusted exam finish time
- Optional fixed break/additional time
- Final scheduled finish time
- A visual timeline showing how the total window is composed
- A clipboard-ready summary

The calculation is performed client-side in the browser.

## Calculation

Extra time is expressed as a percentage.

Adjusted exam minutes = ceiling(standard exam minutes × (1 + extra time ÷ 100))

For example, a 60-minute exam with 50% extra time becomes 90 minutes.

Optional break/fixed time is added after the adjusted exam duration.

## Project structure

- BlazorWasm/Pages/Index.razor — application UI
- BlazorWasm/Pages/Index.razor.cs — UI state, formatting, and interaction logic
- BlazorWasm/Models/ExamCalculation.cs — calculation result model
- BlazorWasm/Services/ExamTimeCalculator.cs — isolated calculation engine
- BlazorWasm/wwwroot/css/app.css — application-wide visual system
- BlazorWasm/wwwroot/js/app.js — clipboard interop
- BlazorWasm/Shared/MainLayout.razor — minimal application shell

## Run locally

Requires the .NET 10 SDK.

From the repository root:

    dotnet run --project BlazorWasm/BlazorWasm.csproj

## Design

The app intentionally does not use the standard Blazor starter navigation, Bootstrap components, or Open Iconic assets. It uses a custom responsive layout, glass-like surfaces, a dark editorial hero, accommodation presets, accessible native form controls, keyboard focus states, and reduced-motion support.
