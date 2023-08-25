using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

/// <summary>
/// Write positions of nodes on a supersynk channel
/// The positions will always increase
/// </summary>
public class WriterThreadWorker
{
    private SuperklubManager manager;
    private int callPerSecond = 10;

    public WriterThreadWorker(IHttpClient httpClient, string serverUrl, string channel, int callPerSecond)
    {
        SupersynkClient supersynkClient = new SupersynkClient(httpClient);
        manager = new SuperklubManager(supersynkClient);
        manager.ServerUrl = serverUrl;
        manager.Channel = channel;

        this.callPerSecond = callPerSecond;
    }

    public async void StartWork()
    {
        //
        int sleepDurationMs = (int)(1000f / callPerSecond);
        Console.WriteLine("Writer StartWork()" + sleepDurationMs);

        // Create local node
        var redBox = new SuperklubNodeRecord();
        redBox.Id = "redbox";
        redBox.Position = (0f, 0f, 0f);
        redBox.Rotation = (0f, 0f, 0f, 1f);
        redBox.Shape = "box";
        redBox.Color = "red";

        // Register local node
        manager.RegisterLocalNode(redBox);

        // Perform first call
        await manager.SynchronizeLocalAndDistantNodes();

        // Iterate
        int iterations = 1000;
        for (int i = 0; i < iterations; i++)
        {
            // Update redbox position
            redBox.Position = ((float)i, 0f, 0f);

            // Log
            Console.WriteLine("Writer loop (" + manager.ClientId + "), " + redBox.Position.x);

            // Do not await response (on purpose)
            manager.SynchronizeLocalAndDistantNodes();

            // Wait before looping
            Thread.Sleep(sleepDurationMs);
        }
    }
}

