using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BasicTest
{
    public static async Task RunTest(string serverUrl, string channel)
    {
        await Scenario01(serverUrl, channel);

        // Wait for disconnection
        Thread.Sleep(6000);

        await Scenario02(serverUrl, channel);
        await Scenario03(serverUrl, channel);
    }

    private static async Task Scenario01(string serverUrl, string channel)
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

        if (res == null)
            Console.WriteLine("- The response is null");
        else
            Console.WriteLine("- The channel contains " + res.Count + " clients");
    }


    private static async Task Scenario02(string serverUrl, string channel)
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


    private static async Task Scenario03(string serverUrl, string channel)
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
}
