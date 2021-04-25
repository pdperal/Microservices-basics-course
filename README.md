## About the project:
This project was build for the Udemy course [Building Microservices with .NET - The Basics](https://www.udemy.com/course/building-microservices-with-net-the-basics/).
The objective of this project is to implement a simple application using microservices architecture to manage Items from a Catalog and bind them to a User Inventory.

## Stack:

This is a simple projet build on top of .NET 5 ecosystem using a distributed architecture, in this case, microservices. Two microservices were build to allow an asynchronous communications using a message broker, such as RabbitMQ.

For this application, the following tecnolgies were used:
- ASP.NET Core
- .NET 5
- C#
- RabbitMQ as a messagebroker
- Mongodb
- Polly for retry and circuit breaker policys
- Docker and Docker-compose

You can run the application using the following command in the .sln path:
- docker-compose up -d

If that doesn't work, pray to god or just send me a message.
