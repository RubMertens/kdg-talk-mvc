// See https://aka.ms/new-console-template for more information


var server = new ThreadingServer();

var slowlyReverseStringInDatabase = (string input) =>
{
    Thread.Sleep(20);
    return input.Reverse();
};

var t = new Thread(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        server.HandleRequest("some data" + i,
            (data) =>
            {
                Console.WriteLine($"BEFORE Handled {data} :: thread ({Thread.CurrentThread.ManagedThreadId})");
                var reverser = new string(slowlyReverseStringInDatabase(data).ToArray());
                Console.WriteLine($"AFTER Handled {reverser} :: thread ({Thread.CurrentThread.ManagedThreadId})");
                return reverser;
            });
    }
});
t.Start();

Console.Read();


public class SingleThreadServer
{
    public void HandleRequest(string req, Func<string, string> handler)
    {
        handler(req);
    }
}

public class ThreadPoolServer
{
    public void HandleRequest(string req, Func<string, string>handler)
    {
        ThreadPool.QueueUserWorkItem((state) => handler(req));
    }
}

public class ThreadingServer
{
    public void HandleRequest(string req, Func<string, string>handler)
    {
        var t =new Thread(() => handler(req));
        t.Start();
        
    }
}