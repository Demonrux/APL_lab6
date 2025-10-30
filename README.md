# Демографическое Моделирование

Проект для демографического моделирования населения с использованием вероятностных методов.

## Описание проекта

Программа имитирует изменение населения с течением времени, учитывая:
- Вероятности смерти по возрастам и полу
- Рождаемость (женщины 18-45 лет)
- Начальное распределение населения по возрастам
- Статистику по годам

## Структура решения
```text
Demographic/ # Решение
├── Demographic/ # Основной проект
│ ├── Classes/
│ │ ├── Engine.cs # Движок симуляции
│ │ ├── Person.cs # Класс человека
│ │ └── ProbabilityCalculator.cs # Калькулятор вероятностей
│ ├── Interfaces/
│ │ └── IEngine.cs # Интерфейс движка
│ └── Models/
│ ├── ChildBirthEventArgs.cs # Аргументы события рождения
│ ├── DeathRules.cs # Правила смертности
│ ├── DemographicStats.cs # Статистика за год
│ ├── Gender.cs # Перечисление полов
│ ├── InitialAgeData.cs # Начальное распределение по возрастам
│ └── SimulationResult.cs # Результаты симуляции
├── Demographic.Exec/ # Запускаемый проект
│ ├── Files/
│ │ ├── DeathRules.csv # Правила смертности
│ │ ├── InitialAge.csv # Начальное распределение возрастов
│ │ ├── PeopleData.csv # Выходные данные людей
│ │ └── PopulationData.csv # Выходные данные населения
│ └── Program.cs # Точка входа
└── Demographic.FileOperations/ # Операции с файлами
├── Classes/
│ ├── CsvDeathRulesReader.cs # Чтение правил смертности
│ ├── CsvInitialAgeReader.cs # Чтение начального распределения
│ └── CsvResultWriter.cs # Запись результатов
└── Interfaces/
├── IDeathRulesReader.cs # Интерфейс чтения правил смертности
├── IInitialAgeReader.cs # Интерфейс чтения возраста
└── IResultWriter.cs # Интерфейс записи результатов
```
## Запуск проекта

### Требования
- .NET 6.0 или выше
- CSV файлы с данными в папке `Demographic.Exec/Files/`

### Командная строка

```bash
cd Demographic.Exec
dotnet run [initialAgeFile] [deathRulesFile] [startYear] [endYear] [totalPopulation] [outputFile1] [outputFile2]
```
## Параметры по умолчанию
- **`initialAgeFile`**: Files/InitialAge.csv
- deathRulesFile`**: Files/DeathRules.csv
- **`startYear`**: 1970
- **`endYear`**: 2021
- **`totalPopulation`**: 130,000,000
- **`outputFile1`**: Files/PopulationData.csv
- **`outputFile2`**: Files/PeopleData.csv

## Форматы входных файлов
### InitialAge.csv
```csv
age,count_per_1000
0,15.2
1,14.8
...
```
### DeathRules.csv
```csv
start_age,end_age,male_probability,female_probability
0,1,0.0316,0.0266
1,4,0.0021,0.0017
...
```
## Выходные данные
### PopulationData.csv
```csv
year,total_population,male_population,female_population
1970,130000000,65000000,65000000
...
```
## PeopleData.csv
```csv
Age,Gender,IsAlive,DeathYear
25,Male,True,
30,Female,False,2005
...
```













