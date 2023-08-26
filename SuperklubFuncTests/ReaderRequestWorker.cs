using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Read positions of nodes on a supersynk channel
/// The positions are expected to always increase
/// </summary>
public class ReaderRequestWorker
{
    SuperklubManager manager;
    Dictionary<string, float> nodePosition;
    Counter lateResponseCounter;
    Counter totalResponseCounter;

    public ReaderRequestWorker(
        SuperklubManager manager,
        Dictionary<string, float> nodePosition,
        Counter lateResponseCounter,
        Counter totalResponseCounter)
    {
        this.manager = manager;
        this.nodePosition = nodePosition;
        this.lateResponseCounter = lateResponseCounter;
        this.totalResponseCounter = totalResponseCounter;
    }

    /// <summary>
    /// Should be executed in its own thread
    /// </summary>
    public async void ExecuteRequest()
    {
        SuperklubUpdate update = await manager.SynchronizeLocalAndDistantNodes();
        if (update.nodesToUpdate.Count > 0)
        {
            foreach (var node in update.nodesToUpdate)
            {
                UpdateNode(node);
            }
        }    
    }

    public void UpdateNode(SuperklubNodeRecord node)
    {
        string nodeId = node.Id;

        lock (nodePosition)
        {
            // Ensure there is a dictionary entry for nodeId
            if (!nodePosition.ContainsKey(nodeId))
            {
                nodePosition.Add(nodeId, 0);
            } 
        }

        //
        totalResponseCounter.Inc();
        Console.WriteLine("    [Reader] " + nodeId + " : received=" + node.Position.x + "    stored=" + nodePosition[nodeId]);

        // Is it a late response ?
        bool isLateresponse = (node.Position.x < nodePosition[nodeId]);
        
        if (isLateresponse)
        {
            lateResponseCounter.Inc();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    [Reader] " + nodeId + " late response (" + lateResponseCounter.Count + "/" + totalResponseCounter.Count + ")");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            lock (nodePosition)
            {
                nodePosition[nodeId] = node.Position.x;
            }
        }
    }
}
