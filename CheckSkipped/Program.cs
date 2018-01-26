using System;
using System.Threading.Tasks;
using MassTransit;

namespace CheckSkipped
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = CreateBus(x =>
            {
                x.Consumer<TestConsumer>();
            });

            bus.Start();
            bus.Stop();

            bus = CreateBus();

            bus.Publish(new DummyMessage());

            bus.Start();

            Console.ReadKey();
        }

        static IBusControl CreateBus(Action<IReceiveEndpointConfigurator> configureEndpoint = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var uri = new Uri("rabbitmq://localhost/");

                cfg.Host(uri, h =>
                {
                });

                cfg.ReceiveEndpoint("Testing", e =>
                {
                    configureEndpoint?.Invoke(e);
                });
            });
        }
    }

    class DummyMessage
    {
    }

    class TestConsumer : IConsumer<DummyMessage>
    {
        public Task Consume(ConsumeContext<DummyMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
