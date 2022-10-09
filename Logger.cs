using System;
using System.IO;

class Program
{
    public static void Main()
    {
        Pathfinder pathfinder1 = new Pathfinder(new FileLogger());
        Pathfinder pathfinder2 = new Pathfinder(new ConsoleLogger());
        Pathfinder pathfinder3 = new Pathfinder(new FridayLogger(new ConsoleLogger()));
        Pathfinder pathfinder4 = new Pathfinder(new FridayLogger(new FileLogger()));
        Pathfinder pathfinder5 = new Pathfinder(new ChainOfLogger(new ILogger[] { new ConsoleLogger(), new FridayLogger(new FileLogger()) }));
    }
}

interface ILogger
{
    public abstract void Log(string massage);
}

class ChainOfLogger : ILogger
{
    private ILogger[] _loggers;

    public ChainOfLogger(ILogger[] loggers) => _loggers = loggers;

    public void Log(string massage)
    {
        foreach (var logger in _loggers)
            logger.Log(massage);
    }
}

class Pathfinder
{
    private ILogger _logger;

    public Pathfinder(ILogger logger) => _logger = logger;

    public void Find(string massage) => _logger.Log(massage);
}

class FileLogger : ILogger
{
    public void Log(string massage) => File.WriteAllText("text.txt", massage);
}

class ConsoleLogger : ILogger
{
    public void Log(string massage) => Console.WriteLine(massage);
}

class FridayLogger : ILogger
{
    private ILogger _logger;

    public FridayLogger(ILogger logger) => _logger = logger;

    public void Log(string massage)
    {
        if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            _logger.Log(massage);
    }
}