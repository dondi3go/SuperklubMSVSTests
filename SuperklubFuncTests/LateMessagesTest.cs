using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LateMessagesTest
{
    /// <summary>
    /// Perform concurrent read/write requests on a channel with or without
    /// late reponses handling and compare the results.
    /// </summary>
    public static void RunTest(string serverUrl, string channel, int callPerSecond)
    {
        Console.WriteLine("Running late messages test");

        // Run scenario WITHOUT late messages handling
        List<int> resultA = RunScenario(serverUrl, channel, callPerSecond, false);

        // Wait for disconnection
        Thread.Sleep(6000);

        // Run scenario WITH late messages handling
        List<int> resultB = RunScenario(serverUrl, channel, callPerSecond, true);

        // Compare result
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Without : " + resultA[0] + " / " + resultA[1]);
        Console.WriteLine("   With : " + resultB[0] + " / " + resultB[1]);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static List<int> RunScenario(string serverUrl, string channel, int callPerSecond, bool handleLate)
    {
        // Create http client (common to everyone)
        IHttpClient httpClient = new MSHttpClient();

        // Create writer #1
        WriterLoopWorker w1 = new WriterLoopWorker(httpClient, serverUrl, channel, callPerSecond, handleLate);
        Thread w1Thread = new Thread(w1.StartLoop);
        w1Thread.Name = "WriterLoopThread#1";

        // Create writer #2
        WriterLoopWorker w2 = new WriterLoopWorker(httpClient, serverUrl, channel, callPerSecond, handleLate);
        Thread w2Thread = new Thread(w2.StartLoop);
        w2Thread.Name = "WriterLoopThread#2";

        // Create reader
        ReaderLoopWorker reader = new ReaderLoopWorker(httpClient, serverUrl, channel, callPerSecond, handleLate);
        Thread readerLoopThread = new Thread(reader.StartLoop);
        readerLoopThread.Name = "ReaderLoopThread";

        // Start all threads
        w1Thread.Start();
        w2Thread.Start();
        readerLoopThread.Start();

        // Wait for all threads to finish
        w1Thread.Join();
        w2Thread.Join();
        readerLoopThread.Join();

        // Get result
        List<int> result = new List<int>();
        result.Add(reader.LateMessagesCounter.Count);
        result.Add(reader.TotalMessagesCounter.Count);

        // Display result
        Console.WriteLine("Late messages = " + result[0] + " / " + result[1]);

        // Return result
        return result;
    }
}

