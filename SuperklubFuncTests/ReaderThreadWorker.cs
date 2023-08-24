using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Read positions of nodes on a supersynk channel
/// The positions are expected to always increase
/// </summary>
public class ReaderThreadWorker
{
    SuperklubManager manager;
    Dictionary<string, float> nodePosition;
    Counter lateResponseCounter;
    Counter totalResponseCounter;

    public ReaderThreadWorker(
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

    public async void DoWork()
    {
        SuperklubUpdate update = await manager.SynchronizeLocalAndDistantNodes();
        if (update.nodesToUpdate.Count > 0)
        {
            foreach (var node in update.nodesToUpdate)
            {
                string nodeId = node.Id;
                if (!nodePosition.ContainsKey(nodeId))
                {
                    lock (nodePosition)
                    {
                        nodePosition[nodeId] = 0;
                    }
                }
                totalResponseCounter.Inc();
                Console.WriteLine("Reader " + nodeId + " : received=" + node.Position.x + "    stored=" + nodePosition[nodeId]);
                if (node.Position.x < nodePosition[nodeId])
                {
                    lateResponseCounter.Inc();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Reader " + nodeId + " late response (" + lateResponseCounter.Count + "/" + totalResponseCounter.Count + ")");
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
    }
}
