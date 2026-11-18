# DataLens Patterns — вариант 13

Учебный репозиторий по дисциплине **«Паттерны проектирования»**, направление 09.03.04 «Программная инженерия».

**Предметная область варианта 13:** «Интерактивный анализ набора данных».

Каждая работа имеет собственную папку, README с описанием решения, отдельный запускаемый .NET-проект, исходный код в `SRC/`, диаграммы классов в `UML/` и контрольные материалы в `assets/`.

## Содержание

| Папка | Паттерны |
|---|---|
| [`Lab1_DatasetAnalysis`](Lab1_DatasetAnalysis/README.md) | Singleton, Builder |
| [`Lab2_DatasetAnalysis`](Lab2_DatasetAnalysis/README.md) | Abstract Factory, Prototype |
| [`Lab3_DatasetAnalysis`](Lab3_DatasetAnalysis/README.md) | Adapter, Bridge |
| [`Lab4_DatasetAnalysis`](Lab4_DatasetAnalysis/README.md) | Composite, Decorator |
| [`Lab5_DatasetAnalysis`](Lab5_DatasetAnalysis/README.md) | Facade, Flyweight |

## Общая архитектура

`Shared/DataLensPatterns.Core` содержит общую модель табличных данных, CSV-загрузчик, валидатор, аналитический сервис и экспортёр. Эта библиотека подключена к проектам пяти лабораторных через `ProjectReference`, поэтому реализация предметной области не дублируется.

```text
DataLensPatterns_Variant13_GitHubStyle/
├── Lab1_DatasetAnalysis/
│   ├── SRC/
│   ├── UML/
│   ├── assets/
│   ├── Reports/
│   ├── README.md
│   ├── Lab1_DatasetAnalysis.sln
│   └── START_PROJECT.bat
├── Lab2_DatasetAnalysis/ ...
├── Lab3_DatasetAnalysis/ ...
├── Lab4_DatasetAnalysis/ ...
├── Lab5_DatasetAnalysis/ ...
├── Shared/DataLensPatterns.Core/
├── datasets/
├── Reports/
├── DataLensPatterns.sln
├── Directory.Build.props
└── RUN_ALL.bat
```

## Требования

- Windows 10/11;
- .NET 8 SDK;
- Visual Studio 2022 либо PowerShell/Windows Terminal.

Сторонние NuGet-пакеты не требуются. В проектах используется C# 12.

## Сборка всех проектов

Из корневой папки:

```powershell
dotnet --version
dotnet build .\DataLensPatterns.sln
```

## Запуск

Для отдельной работы запустите `START_PROJECT.bat` в папке соответствующей лабораторной. 

Также можно запускать проекты командами:

```powershell
dotnet run --project .\Lab1_DatasetAnalysis\SRC\Lab1_BuilderSingleton.csproj
dotnet run --project .\Lab2_DatasetAnalysis\SRC\Lab2_AbstractFactoryPrototype.csproj
dotnet run --project .\Lab3_DatasetAnalysis\SRC\Lab3_AdapterBridge.csproj
dotnet run --project .\Lab4_DatasetAnalysis\SRC\Lab4_CompositeDecorator.csproj
dotnet run --project .\Lab5_DatasetAnalysis\SRC\Lab5_FacadeFlyweight.csproj
```

## Демонстрационные данные

`datasets/sample_sales.csv` и `datasets/sample_sales.json` содержат один и тот же набор из 10 записей и 6 полей. В исходном CSV 60 ячеек, 2 пропуска и заполненность 96,67%. Фильтр по `revenue` в диапазоне 20 000–90 000 включительно возвращает 8 строк; в отфильтрованной таблице 1 пропуск и заполненность 97,92%.

## Отчёты и UML

- `LabN_DatasetAnalysis/README.md` — развёрнутое описание конкретной лабораторной в формате, близком к образцу репозитория;
- `LabN_DatasetAnalysis/UML/` — отдельные диаграммы каждого применённого паттерна (`.png` и `.dot`);
- `LabN_DatasetAnalysis/Reports/` — формальный отчёт по лабораторной в DOCX и PDF;
- `Reports/` — общий отчёт и PDF со всеми работами.

## Ограничение проверки сборки

Структура, пути проектов и содержимое исходных файлов подготовлены для .NET 8.