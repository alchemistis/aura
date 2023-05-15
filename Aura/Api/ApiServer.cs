using System.Net;

namespace Aura.Api;

public class ApiServer
{
    private readonly HttpListener _httpListener;
    private readonly IDictionary<ApiRoute, Action<ApiRoute, HttpListenerResponse>?> _handlers;

    public ApiServer()
    {
        _httpListener = new HttpListener();
        _handlers = new Dictionary<ApiRoute, Action<ApiRoute, HttpListenerResponse>?>();
    }

    public void RegisterEndpoint(string route, string method, Action<ApiRoute, HttpListenerResponse>? handler)
    {
        var apiRoute = new ApiRoute(route, new HttpMethod(method));
        _handlers.Add(apiRoute, handler);
    }
    
    public async Task Start()
    {
        _httpListener.Prefixes.Add("http://*:8001/");
        _httpListener.Start();

        Console.WriteLine("Local HTTP server started...");
        
        await Task.Run(() =>
        {
            while (_httpListener.IsListening)
            {
                var context = _httpListener.GetContext();

                Console.WriteLine("Request received...");
                
                var requestRoute = context.Request.RawUrl;
                var requestMethod = context.Request.HttpMethod;

                if (requestRoute is null)
                {
                    return;
                }

                var route = new ApiRoute(requestRoute, new HttpMethod(requestMethod));
                if (_handlers.TryGetValue(route, out var handler))
                {
                    handler?.Invoke(route, context.Response);
                }
            }
        });
    }
}