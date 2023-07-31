using Superklub;

async Task Scenario01()
{
    Console.WriteLine("Running Scenario 01 (Supersynk basic requests) :");

    // Create client
    IHttpClient httpClient = new MSHttpClient(
        new Uri("http://127.0.0.1:9999"),
        "api/channels", "default");
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);
    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    // Create minimal data for the server
    SupersynkClientDTO dto = new SupersynkClientDTO("ada");

    // Start exchanges with the server

    Console.WriteLine("- Sending POST request to server");
    await supersynkClient.PostAsync(dto);

    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    Console.WriteLine("- Sending GET request to server");
    var res = await supersynkClient.GetAsync();

    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    Console.WriteLine("- The channel contains " + res.List.Count + " clients");
}


async Task Scenario02()
{
    Console.WriteLine("Running Scenario 03 (Superklub basic participant) :");

    // Create client
    IHttpClient httpClient = new MSHttpClient(
         new Uri("http://127.0.0.1:9999"),
         "api/channels", "default");
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);

    // Create superklub manager
    Console.WriteLine("- Creating Superklub manager");
    SuperklubManager manager = new SuperklubManager(supersynkClient);

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

async Task Scenario03()
{
    Console.WriteLine("Running Scenario 04 (Superklub basic observer) :");

    // Create client
    IHttpClient httpClient = new MSHttpClient(
        new Uri("http://127.0.0.1:9999"),
        "api/channels", "default");
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);

    // Create superklub manager
    Console.WriteLine("- Creating Superklub manager");
    SuperklubManager manager = new SuperklubManager(supersynkClient);

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

await Scenario03();

/*
class MainClass
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting functionnal tests");
        Console.WriteLine(args.Length);
        if (args.Length == 1)
        {
            int parameter = 0;
            if (int.TryParse(args[0], out parameter))
            {
                Console.WriteLine(parameter);
            }
        }
        Console.WriteLine(args.Length);
    }
}*/


