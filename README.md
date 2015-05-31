# AMPS - Agile Music Project Suite

## Predefined Port: **5004** (Can be changed in [`Server.cs`](https://github.com/EdAllonby/AMPS/blob/master/Source/Server/Server.cs))

### To Run a demo of AMPS:

#### Prerequisites:
* **To run** - Must have .Net 4.5 or later installed to run the Server and the Client. Download .Net 4.5 from: https://www.microsoft.com/en-gb/download/details.aspx?id=30653
* **To build** - Must have Nuget package restore to download essential assemblies such as Log4net, NUnit and some WPF controls. 

#### Guide:
1. Build and run the Server project.
2. When the AMPS Server is running, Press '2' to start the server without using a database implementation. This will use an in-memory persistence strategy for domain entities.
3. Start the client by building and running the `Client.View` project. 
4. A login screen will be displayed. Choose a Username and a Password. Then input:
	* IP Address: **127.0.0.1**
	* Port: **5004**
5. You may connect many clients to a server session, and run common tasks such as starting a multi-user band and adding tasks to a Jam.

If you want to run server on a seperate network, you'll need to set up port forwarding.

### Creating a Database
Scripts are located in Server/Database_Scripts. (Need to make this 1 easy script to run, see Issue #2). Once the database is created, you can point the Server to the database (defined in: Server/app.config) and then run Option 1 on the server to gain persistence.

Thanks, Ed.
