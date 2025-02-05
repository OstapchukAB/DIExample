using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;


//service 2
public class Engine:IEngine
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


        // Создаем коллекцию сервисов (контейнер).
        var serviceCollection = new ServiceCollection();

        //Регистрируем зависимости с реализациями
        serviceCollection.AddTransient<IEngine, Engine>();
       // serviceCollection.AddTransient<IEngine, EngineElectric>();

        //Регистрируем клиента
        serviceCollection.AddTransient<Car>();

        //Построение провайдера - инжектора зависимостей
        var serviceProvider= serviceCollection.BuildServiceProvider();

        //Получаем экземпляр Car с внедренной зависимостью
        var car = serviceProvider.GetService<Car>();

        //Используем объект
        car.StartCar();
        car.StarStop();

    }

}


