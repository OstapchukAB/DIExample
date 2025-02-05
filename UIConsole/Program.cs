using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;


//service 2
public class EnginePetrol : IEngine
{
    public void Start() => Console.WriteLine("Двигатель (ДВГ) запущен");
    public void Stop() => Console.WriteLine("Двигатель (ДВГ) остановлен");
}
//service 1
public class EngineElectric : IEngine
{
    public void Start() => Console.WriteLine("Двигатель (электро) запущен");
    public void Stop() => Console.WriteLine("Двигатель (электро) остановлен");
}

public interface IEngine
{
    public void Start();
    public void Stop();
}
// Интерфейс фабрики для создания двигателей
public interface IEngineFactory
{
    IEngine CreateEngine(EngineType engineType);
}
// Фабрика, которая инкапсулирует логику выбора реализации
public class EngineFactory : IEngineFactory
{
    private readonly IServiceProvider _provider;

    public EngineFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IEngine CreateEngine(EngineType engineType)
    {
        // В зависимости от переданного параметра выбираем нужную реализацию
        switch (engineType)
        {
            case EngineType.Petrol:
                return _provider.GetRequiredService<EnginePetrol>();
            case EngineType.Electric:
                return _provider.GetRequiredService<EngineElectric>();
            default:
                throw new ArgumentException("Неверный тип двигателя", nameof(engineType));
        }
    }
}


//client
public class Car
{

    private readonly IEngine _engine;

    public Car(IEngine engine)
    {
        _engine = engine;
    }

    public void StartCar()
    {
        // Используем зависимость
        _engine.Start();
        Console.WriteLine("Машина поехала");
    }
    public void StarStop()
    {
        // Используем зависимость
        _engine.Stop();
        Console.WriteLine("Машина остановилась");
    }
}

// Перечисление для типов двигателей
public enum EngineType
{
    Petrol,
    Electric
}

public class Program
{
    public static void Main()
    {

        //Таким образом, с помощью DI-контейнера мы управляем созданием зависимостей,
        //а выбор конкретной реализации(бензиновой или электрической) происходит на этапе разрешения сервиса.
        //Это позволяет гибко переключаться между различными реализациями в одном коде.


        // Создаем коллекцию сервисов
        var services = new ServiceCollection();

        // Регистрируем конкретные реализации двигателей.
        // Обратите внимание, что регистрируем их как конкретные типы, а не через интерфейс IEngine,
        // чтобы потом можно было различать их.
        services.AddTransient<EnginePetrol>();
        services.AddTransient<EngineElectric>();

        // Регистрируем фабрику. Она будет использовать IServiceProvider для разрешения зависимостей.
        services.AddSingleton<IEngineFactory, EngineFactory>();

        // Регистрация Car не обязательна, если мы будем создавать его вручную через new Car(...)
        // или можем зарегистрировать и потом использовать фабрику для создания двигателя, например:
        // services.AddTransient<Car>();


        // Построение DI-контейнера (инжектор)
        var serviceProvider = services.BuildServiceProvider();

        // Получаем фабрику из DI-контейнера
        var engineFactory = serviceProvider.GetRequiredService<IEngineFactory>();


        Console.WriteLine("введите тип двигателя 0- Бензин 1 -Электро");
        var s = Console.ReadLine();
        if (int.TryParse(s, out int val))
        {

            // Создаем и используем Car с бензиновым двигателем
            IEngine engine = val switch
            {
                0 => engineFactory.CreateEngine(EngineType.Petrol),
                1 => engineFactory.CreateEngine(EngineType.Electric),
                _ => throw new NotImplementedException()
            };

            var car = new Car(engine);
            Console.WriteLine("Попытка запуска двигателя:");
            car.StartCar();
            Console.WriteLine("Попытка остановки двигателя:");
            car.StarStop();

            Console.WriteLine();
        }
    }

}


