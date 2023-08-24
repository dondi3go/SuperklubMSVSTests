using Superklub;
using System.Threading.Channels;
using static System.Net.WebRequestMethods;


async Task RunSupersynkPerfTest(string serverUrl, string channel)
{
    Console.WriteLine("Running Supersynk POST performance test : ");

    // Create client
    IHttpClient httpClient = new MSHttpClient();
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);

    // Url for requests
    string url = serverUrl + "/api/channels/" + channel;

    // Create data to send to the server
    SupersynkClientDTO dto = new SupersynkClientDTO("ada");

    // Send the first request
    Console.WriteLine("- Performing first POST requests to the server");
    await supersynkClient.PostAsync(url, dto);

    // Show server status
    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    // Prepare loop
    int requestCount = 500;
    Console.WriteLine("- Performing " + requestCount + " POST requests to the server...");

    // Start watch
    var watch = System.Diagnostics.Stopwatch.StartNew();

    // Perform requests
    for (int i = 0; i < requestCount; i++)
    {
        await supersynkClient.PostAsync(url, dto);
    }

    // Stop watch
    watch.Stop();

    // Display result
    float avg = (float)watch.ElapsedMilliseconds / (float)requestCount;
    Console.WriteLine("- Average time per request = " + avg + " ms");
}

async Task RunSuperklubPerfTest(string serverUrl, string channel)
{
    Console.WriteLine("Running Superklub synchronization performance test : ");

    // Create client
    IHttpClient httpClient = new MSHttpClient();
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);

    // Create superklub manager
    Console.WriteLine("- Creating Superklub manager");
    SuperklubManager manager = new SuperklubManager(supersynkClient);
    manager.ServerUrl = serverUrl;
    manager.Channel = channel;

    // Create local node
    var redBox = new SuperklubNodeRecord();
    redBox.Id = "redbox";
    redBox.Position = (0.1f, 0.2f, 0.3f);
    redBox.Rotation = (0f, 0f, 0f, 1f);
    redBox.Shape = "box";
    redBox.Color = "red";

    // Register local node
    manager.RegisterLocalNode(redBox);

    // Send the first request
    Console.WriteLine("- Performing first synchronization with the server");
    await manager.SynchronizeLocalAndDistantNodes();

    // Show server status
    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    // Prepare loop
    int requestCount = 500;
    Console.WriteLine("- Performing " + requestCount + " synchronizations with the server...");

    // Start watch
    var watch = System.Diagnostics.Stopwatch.StartNew();

    // Perform requests
    for (int i = 0; i < requestCount; i++)
    {
        await manager.SynchronizeLocalAndDistantNodes();
    }

    // Stop watch
    watch.Stop();

    // Display result
    float avg = (float)watch.ElapsedMilliseconds / (float)requestCount;
    Console.WriteLine("- Average time per request = " + avg + " ms");
}

string serverUrl = "http://127.0.0.1:5000";
string channel = "default";

await RunSuperklubPerfTest(serverUrl, channel);
await RunSupersynkPerfTest(serverUrl, channel);

