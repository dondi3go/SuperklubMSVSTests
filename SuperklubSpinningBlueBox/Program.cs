using Superklub;
using System.Numerics;

//
// Send to Superklub a blue box spinning around origin in the XZ plane
// Note : url and port are hardcoded for the time being
//
async Task RunSpinningBlueBox()
{
    Console.WriteLine("Running Superklub blue box spinning :");

    // Create client
    IHttpClient httpClient = new MSHttpClient(
        new Uri("http://127.0.0.1:9999"),
        "api/channels", "default");
    SupersynkClient supersynkClient = new SupersynkClient(httpClient);

    // Create superklub manager
    Console.WriteLine("- Creating Superklub manager");
    SuperklubManager manager = new SuperklubManager(supersynkClient);

    // Create local node
    var blueBox = new SuperklubNodeRecord();
    blueBox.Id = "bluebox";
    blueBox.Position = (0.1f, 0.2f, 0.3f);
    blueBox.Rotation = (0f, 0f, 0f, 1f);
    blueBox.Shape = "box";
    blueBox.Color = "blue";

    // Register local node to Superklub manager
    manager.RegisterLocalNode(blueBox);

    // Send the first request
    Console.WriteLine("- Performing first synchronization with the server");
    await manager.SynchronizeLocalAndDistantNodes();

    // Show server status
    Console.WriteLine("- The server status is " + supersynkClient.Status.ToString());

    double angleRad = 0f; // radians
    double speedRadPerSec = Math.PI / 3; // radians per second
    DateTime startTime = DateTime.Now;
    while (true)
    {
        // Change node position
        var timeFromStartSec = (DateTime.Now - startTime).TotalSeconds;
        angleRad = speedRadPerSec * timeFromStartSec;
        blueBox.Position = ((float)Math.Cos(angleRad), 0f, (float)Math.Sin(angleRad));

        // Update server
        Console.WriteLine("- Updating server, blue box position is now " + blueBox.Position.ToString());
        await manager.SynchronizeLocalAndDistantNodes();

        // Wait between two updates 
        Thread.Sleep(100);
    }
}

await RunSpinningBlueBox();
