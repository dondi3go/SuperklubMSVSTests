using Superklub;
using System.Diagnostics.Metrics;
using static System.Net.WebRequestMethods;

async Task Scenario01(string serverUrl, string channel)
{
    Console.WriteLine("Running Scenario 01 (Supersynk basic requests) :");

    // Create client
    IHttpClient httpClient = new MSHttpClient();
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);
    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    // Url
    string url = serverUrl + "/api/channels/" + channel;

    // Create minimal data for the server
    SupersynkClientDTO dto = new SupersynkClientDTO("ada");

    // Start exchanges with the server

    Console.WriteLine("- Sending POST request to server");
    await supersynkClient.PostAsync(url, dto);

    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    Console.WriteLine("- Sending GET request to server");
    SupersynkClientDTOs? res = await supersynkClient.GetAsync(url);

    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());
    
    if(res == null)
        Console.WriteLine("- The response is null");
    else
        Console.WriteLine("- The channel contains " + res.Count + " clients");
}

async Task Scenario02(string serverUrl, string channel)
{
    Console.WriteLine("Running Scenario 02 (Superklub basic participant) :");

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

    // Perform one synchronization with the server
    Console.WriteLine("- Performing Superklub synchronization as a participant");
    SuperklubUpdate update = await manager.SynchronizeLocalAndDistantNodes();

    // Check result of synchronisation
    Console.WriteLine("- Disconnected clients = " + update.disconnectedClients.Count);
    Console.WriteLine("- New connected clients = " + update.newConnectedClients.Count);
    Console.WriteLine("- Nodes to create = " + update.nodesToCreate.Count);
    Console.WriteLine("- Nodes to update = " + update.nodesToUpdate.Count);
    Console.WriteLine("- Nodes to delete = " + update.nodesToDelete.Count);
}

async Task Scenario03(string serverUrl, string channel)
{
    Console.WriteLine("Running Scenario 03 (Superklub basic observer) :");

    // Create client
    IHttpClient httpClient = new MSHttpClient();
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);

    // Create superklub manager
    Console.WriteLine("- Creating Superklub manager");
    SuperklubManager manager = new SuperklubManager(supersynkClient);
    manager.ServerUrl = serverUrl;
    manager.Channel = channel;

    // Perform one synchronization with the server
    Console.WriteLine("- Performing Superklub synchronization as an observer");
    SuperklubUpdate update = await manager.SynchronizeLocalAndDistantNodes();

    // Check result of synchronisation
    Console.WriteLine("- Disconnected clients = " + update.disconnectedClients.Count);
    Console.WriteLine("- New connected clients = " + update.newConnectedClients.Count);
    Console.WriteLine("- Nodes to create = " + update.nodesToCreate.Count);
    Console.WriteLine("- Nodes to update = " + update.nodesToUpdate.Count);
    Console.WriteLine("- Nodes to delete = " + update.nodesToDelete.Count);
}



async Task Scenario04(string serverUrl, string channel, int callPerSecond)
{
    Console.WriteLine("Running Scenario 04 (Superklub late responses test) :");

    // Create http client (common to everyone)
    IHttpClient httpClient = new MSHttpClient();
    
    // Create WriterThreadWorker #1
    WriterThreadWorker worker1 = new WriterThreadWorker(httpClient, serverUrl, channel, callPerSecond);
    Thread thread1 = new Thread(worker1.StartWork);
    thread1.Name = "thread#1";
    thread1.Start();

    // Create WriterThreadWorker #2
    WriterThreadWorker worker2 = new WriterThreadWorker(httpClient, serverUrl, channel, callPerSecond);
    Thread thread2 = new Thread(worker2.StartWork);
    thread2.Name = "thread#2";
    thread2.Start();

    // Create SuperklubManager for ReaderThreadWorkers
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);
    SuperklubManager manager = new SuperklubManager(supersynkClient);
    manager.ServerUrl = serverUrl;
    manager.Channel = channel;

    // Store latest value of x node position
    Dictionary<string, float> nodePosition = new Dictionary<string, float>();

    // Prepare time span
    double timeSpanSec = 1f / callPerSecond;
    Console.WriteLine("Reader loop : every " + timeSpanSec + " s");

    // Count late responses
    Counter lateResponseCounter = new Counter();
    Counter totalResponseCounter = new Counter();

    // Use periodic timer
    var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(timeSpanSec));
    while (await periodicTimer.WaitForNextTickAsync())
    {
        ReaderThreadWorker w = new ReaderThreadWorker(
            manager, 
            nodePosition, 
            lateResponseCounter,
            totalResponseCounter);
        Thread thread = new Thread(w.DoWork);
        thread.Name = "reader thread";
        thread.Start();
    }
}



//
//
//

string serverUrl = "http://127.0.0.1:5000";
await Scenario04(serverUrl, "linearProgression", 50);

/*string serverUrl = "http://127.0.0.1:5000";
await Scenario02(serverUrl, "functionnalTest");
await Scenario03(serverUrl, "functionnalTest");*/

