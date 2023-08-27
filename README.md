# SuperklubStudioTests

Tests (Unit, functionnal and performance tests) for **Superklub** with Visual Studio

## Installation

This project uses **Superklub** as a submodule

Do not forget to run `git submodule update --init --recursive` after cloning.

## How to use it

* SuperklubUnitTests
    * Run `Test > Execute all tests` on Visual Studio
* SuperklubPerfTests
    * Run the **supersynk** server (See supersynk repo)
    * Set SuperklubPerfTests as startup project
    * Run it
    * It connects to the server and provides metrics about SupersynkClient (the lower layer of Superklub) and Superklub itself
* SuperklubFuncTests
    * Run the **supersynk** server (See supersynk repo)  
    * Set SuperklubFuncTests as the startup project
    * Run it
    * It connects to the server
    * Compare project logs with server logs
* SuperklubSpiningBlueBox
    * This one is usefull to the **SuperklubUnityTests** (see repo)
    * Run the **supersynk** server (See supersynk repo)
    * Run the **SuperklubUnityTests** app
    * Run SuperklubSpiningBlueBox
    * You should see the updates on the server logs
    * You should see a rotating blue cube on the Unity app 
