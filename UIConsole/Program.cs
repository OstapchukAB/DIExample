//service 2
using System.Diagnostics.CodeAnalysis;

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
    // Прямая зависимость: класс сам создает экземпляр Engine.
   // private Engine _engine = new Engine();
   
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
        #region Injector


        Console.WriteLine("выберите тип двигателя:  1 - Электрический, 2 - ДВГ");
        var en=Console.ReadLine();
        var parse=int.TryParse(en, out int result);
        if (parse ==false || result <= 0 || result > 2)
        {
            return;
        }
        // Создание сервиса
        IEngine engine = (TypeEngine)result switch
        {
            TypeEngine.ElectricEngine => new EngineElectric(),
            TypeEngine.PetrolEngine => new Engine(),
            _ => throw new NotImplementedException(),
        };
        Car car = new Car(engine);// Внедрение зависимости в клиента
        #endregion
        
        car.StartCar();
        car.StarStop();
    }
}


