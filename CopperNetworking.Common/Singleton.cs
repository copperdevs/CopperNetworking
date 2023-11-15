namespace CopperNetworking.Common;

public class Singleton<T> where T : new()
{
    private T instance
    {
        get => Instance;
        set => Instance = value;
    }
    public static T Instance { get; private set; }
    private static bool Initialized { get; set; }
    
    public Singleton()
    {
        if (Initialized)
        {
            Log.Error($"Trying to create a {nameof(T)} singleton when one is already created.");
            return;
        }
        Initialized = true;
        
        instance = new T();
    }
}