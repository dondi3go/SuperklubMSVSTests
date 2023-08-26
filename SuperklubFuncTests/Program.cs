using Superklub;
using System.Diagnostics.Metrics;
using System.Threading;
using static System.Net.WebRequestMethods;

//
//
//
string serverUrl = "http://127.0.0.1:5000";

//
//
//
//await BasicTest.RunTest(serverUrl, "basicTest");

//
// This test takes time (2 sesions of 1000 requests).
// The difference is obvious with a server deployed on a VPS.
// On a server running on 127.0.0.1, late messages aren't usually seen.
//
LateMessagesTest.RunTest(serverUrl, "lateMessagesTest", 40);

