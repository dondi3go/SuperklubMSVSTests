using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Run 1000 write requests to a channel
/// </summary>
public class WriterLoopWorker
{
    SupersynkClient supersynkClient;
    SuperklubManager manager;

    // Time span between two write requests (in milliseconds)
    int timeSpanMillisec;

    // Local node
    SuperklubNodeRecord redBox;

    /// <summary>
    /// 
    /// </summary>
    public WriterLoopWorker(
        IHttpClient httpClient,
        string serverUrl,
        string channel,
        int callPerSecond,
        bool handleLate
        )
    {
        //
        supersynkClient = new SupersynkClient(httpClient);
        supersynkClient.HandleLateMessages = handleLate;

        //
        manager = new SuperklubManager(supersynkClient);
        manager.ServerUrl = serverUrl;
        manager.Channel = channel;

        // Prepare time span
        timeSpanMillisec = (int)(1000f / callPerSecond);

        // Create local node
        redBox = new SuperklubNodeRecord();
        redBox.Id = "redbox";
        redBox.Position = (0f, 0f, 0f);
        redBox.Rotation = (0f, 0f, 0f, 1f);
        redBox.Shape = "box";
        redBox.Color = "red";

        // Register local node
        manager.RegisterLocalNode(redBox);
    }

    /// <summary>
    /// Should be executed in its own thread
    /// </summary>
    public void StartLoop()
    {
        Console.WriteLine("[WriterLoop] : loop every " + timeSpanMillisec + " ms");
        
        for(int i=0; i<1000; i++)
        {
            // Update redbox position
            redBox.Position = ((float)i, 0f, 0f);

            // Log
            Console.WriteLine("[WriterLoop] " + manager.ClientId + ":redBox :  sent=" + redBox.Position.x);

            // Do not await response (on purpose)
            manager.SynchronizeLocalAndDistantNodes();

            Thread.Sleep(timeSpanMillisec);
        }

        Console.WriteLine("[WriterLoop] : finished");
    }
}

