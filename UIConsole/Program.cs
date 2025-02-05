using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;


//service 2
public class EnginePetrol:IEngine
{
    public void Start()=> Console.WriteLine("Двигатель (ДВГ) запущен");
    public void Stop() => Console.WriteLine("Двигатель (ДВГ) остановлен");
}
//service 1
public class EngineElectric:IEngine
{
    public void Start() => Console.WriteLine("Двигатель (электро) запущен");
    public void Stop() => Console.WriteLine("Двигатель (электро) остановлен");
}

public interface IEngine
{
    public void Start(); 
    public void Stop();
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

public enum TypeEngine {ElectricEngine=1,PetrolEngine=2 }
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

        // Регистрируем клиента. Если Car зависит от IEngine, то его разрешение не будет однозначным,
        // поэтому мы будем создавать Car вручную, передавая нужную реализацию.
        services.AddTransient<Car>();

        // Построение DI-контейнера (инжектор)
        var serviceProvider = services.BuildServiceProvider();

        // 1. Создаем и используем Car с бензиновым двигателем
        var petrolEngine = serviceProvider.GetService<EnginePetrol>();
        // Здесь мы вручную создаем экземпляр Car с нужной реализацией IEngine.
        var carWithPetrol = new Car(petrolEngine);
        Console.WriteLine("Запуск машины с бензиновым двигателем:");
        carWithPetrol.StartCar();

        Console.WriteLine();

        // 2. Создаем и используем Car с электрическим двигателем
        var electricEngine = serviceProvider.GetService<EngineElectric>();
        var carWithElectric = new Car(electricEngine);
        Console.WriteLine("Запуск машины с электрическим двигателем:");
        carWithElectric.StartCar();


    }

}


