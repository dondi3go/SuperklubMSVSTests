using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Run 1000 read requests to a channel and count
/// - the late responses
/// - the correct responses
/// </summary>
public class ReaderLoopWorker
{
    SupersynkClient supersynkClient;
    SuperklubManager manager;

    // Time span between two read requests (in milliseconds)
    int timeSpanMillisec;

    // Store latest value of x node position
    Dictionary<string, float> nodePosition;

    //
    public Counter lateResponseCounter { get; } = new Counter();
    public Counter totalResponseCounter { get; } = new Counter();

    /// <summary>
    /// 
    /// </summary>
    public ReaderLoopWorker(
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

        // Store latest value of x node position
        nodePosition = new Dictionary<string, float>();

        // Prepare time span
        timeSpanMillisec = (int)(1000f / callPerSecond);
    }

    /// <summary>
    /// Should be executed in its own thread
    /// </summary>
    public void StartLoop()
    {
        Console.WriteLine("[ReaderLoop] : loop every " + timeSpanMillisec + " ms");
        
        for(int i=0; i<1000; i++)
        {
            ReaderRequestWorker w = new ReaderRequestWorker(
                manager,
                nodePosition,
                lateResponseCounter,
                totalResponseCounter);
            Thread thread = new Thread(w.ExecuteRequest);
            thread.Name = "reader thread";
            thread.Start();

            Thread.Sleep(timeSpanMillisec);
        }

        Console.WriteLine("[ReaderLoop] : finished");
    }
}

