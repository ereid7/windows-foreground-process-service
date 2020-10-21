# Windows Foreground Process Tracking/Logging Service

## Description
A .NET Core Worker Service used for logging foreground process history and tracking foreground process duration.

## Features
- Store daily foreground process information (such as foreground duration) in local XML store
- Log foreground process update information to daily CSV log
- Creates new directory in AppData for data/logs each day

## 3rd Party NuGet Packages
- CsvHelper

## Unit Tests
Unit tests live in the `AppTimerService.UnitTests` project
