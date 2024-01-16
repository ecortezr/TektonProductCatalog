// See https://aka.ms/new-console-template for more information
using ProductProducer;

// Console.WriteLine("Hello, World!");

var produceMessage = new ProduceMessage();
produceMessage.CreateMessage().Wait();