# WebApi-as-Plugin POC

A PoC of asp net core api's in a Plugin-like system.

## Example

Build or publish the two projects under `ExampleProjects` and place their folders in an easy-to-remember directory.

Open `PluginApiDemo.sln` and run the project. 

Make a new request like the following `http://localhost:5000/load/?name=ApiOne&dir=C:/Publish/FolderOfProject1`. The response you should get is `Api ApiOne loaded`.

Make a new get request to `http://localhost:5000/Home`, the response you should get is `DemoApiOne`.

Repeat the following for the second project, at second step make the request to `http://localhost:5000/Test`.

You can have 2 or more Web Api loaded as long they dont use the same request handle.

In order to remove an api you make a request like `http://localhost:5000/remove/?name=ApiOne`.

The 2 test projects is .net core libraries, using .net core 3.1, if you want to create your own api use the same specs and add the `Microsoft.AspNetCore.Mvc.Core` from nuget.