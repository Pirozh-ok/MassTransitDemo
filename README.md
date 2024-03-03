# Saga demonstration
In this repo, the Saga orchestration will be covered, and a simple scenario will be implemented to indicate how it works. In the previous article, MassTransit covered and demonstrated how can send a message through MassTransit and RabbitMQ. The code is available on GitHub and if there are any questions, please feel free to visit my website and contact me. So, let's get started.

## Scenario
In this scenario, three services are going to be used to illustrate the process of performing the saga pattern and managing the consistency of data. The services are:
TicketService: Responsible for registering a ticket
GenerateTicketService: Responsible for generating a ticket
EmailService: Responsible for sending ticket information to the user (to keep the example simple, logging information will be used)

As shown in the above image, a client will send a request to TicketService to register a ticket, then a AddTicketEvent will occur and send a message to GenerateTicketService and if the message contains valid values, it will send a message to EmailService to send a confirmation email that contains all information of ticket to the client. If one of the Services of GenerateTicketService or EmailService retrieves an invalid message the Cancel event will occur and incoming data will be removed from all Service's database.

## Prerequires
- C# and .NET Core
- Visual Studio
- Entity Framework core
- MSSQL Server
- MassTransit
- RabbitMQ
- Docker
- Postman
