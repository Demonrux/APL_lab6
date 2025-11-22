/*! 
\mainpage Demographic Modeling System - Архитектура

@section overview Обзор системы

Демографическая система моделирования состоит из трех основных модулей:

- **Ядро системы** (Engine, Person, ProbabilityCalculator)
- **Файловые операции** (CSV читатели/писатели)  
- **Модели данных** (Data classes)

@dot
digraph Architecture {
    rankdir=TB;
    node [shape=rectangle, style=filled, fontname=Arial];
    
    // Основное ядро
    subgraph cluster_core {
        label = "Основная логика";
        color=blue;
        fontname=Arial;
        
        Engine [fillcolor=lightblue];
        Person [fillcolor=lightblue];
        ProbabilityCalculator [fillcolor=lightblue];
    }
    
    // Файловые операции
    subgraph cluster_fileops {
        label = "Файловые операции";
        color=green;
        fontname=Arial;
        
        CsvDeathRulesReader [fillcolor=lightgreen];
        CsvInitialAgeReader [fillcolor=lightgreen]; 
        CsvResultWriter [fillcolor=lightgreen];
    }
    
    // Модели данных
    subgraph cluster_models {
        label = "Модели данных";
        color=orange;
        fontname=Arial;
        
        DeathRules [fillcolor=lightyellow];
        InitialAgeData [fillcolor=lightyellow];
        Constants [fillcolor=lightyellow];
        DemographicStats [fillcolor=lightyellow];
        SimulationResult [fillcolor=lightyellow];
        ChildBirthEventArgs [fillcolor=lightyellow];
        DeathRule [fillcolor=lightyellow];
    }
    
    // Интерфейсы
    subgraph cluster_interfaces {
        label = "Интерфейсы";
        color=purple;
        fontname=Arial;
        
        IEngine [fillcolor=plum];
        IDeathRulesReader [fillcolor=plum];
        IInitialAgeReader [fillcolor=plum];
        IResultWriter [fillcolor=plum];
    }
    
    Engine -> Person [label="содержит"];
    Engine -> DeathRules [label="использует"];
    Engine -> InitialAgeData [label="использует"];
    Engine -> Constants [label="использует"];
    Engine -> SimulationResult [label="генерирует"];
    
    Person -> DeathRules [label="проверка смерти"];
    Person -> ProbabilityCalculator [label="вероятности"];
    Person -> Constants [label="использует"];
    Person -> ChildBirthEventArgs [label="генерирует"];
    
    // Связи файловых операций
    CsvDeathRulesReader -> DeathRules [label="читает"];
    CsvInitialAgeReader -> InitialAgeData [label="читает"]; 
    CsvResultWriter -> DemographicStats [label="пишет"];
    CsvResultWriter -> SimulationResult [label="пишет"];
    
    // Реализации интерфейсов
    Engine -> IEngine [label="реализует", style=dashed];
    CsvDeathRulesReader -> IDeathRulesReader [label="реализует", style=dashed];
    CsvInitialAgeReader -> IInitialAgeReader [label="реализует", style=dashed];
    CsvResultWriter -> IResultWriter [label="реализует", style=dashed];
}
@enddot

@section components Основные компоненты

@subsection core Ядро системы
- Engine - основной движок симуляции
- Person - модель человека
- ProbabilityCalculator - калькулятор вероятностей

@subsection models Модели данных
- DeathRules - правила смертности
- InitialAgeData - начальное распределение возрастов
- Constants - константы системы
- DemographicStats - статистика за год
- SimulationResult - результаты симуляции
- ChildBirthEventArgs - данные о рождении ребенка
- DeathRule - правило смертности

@subsection fileops Файловые операции
- CsvDeathRulesReader - чтение правил смертности
- CsvInitialAgeReader - чтение начального распределения возрастов  
- CsvResultWriter - запись результатов
*/