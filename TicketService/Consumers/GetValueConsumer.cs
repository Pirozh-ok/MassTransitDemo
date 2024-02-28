using Events.TicketEvents;
using MassTransit;

namespace TicketService.Consumers {
    public class GetValueConsumer : IConsumer<IGETValueEvent> {
        private readonly ILogger<GetValueConsumer> _logger;

        public GetValueConsumer(ILogger<GetValueConsumer> logger) {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IGETValueEvent> context) {
            var data = context.Message;

            if (data is not null) {
                await context.Publish<IAddTicketEvent>(new {
                    TicketId = data.TicketId,
                    Title = data.Title,
                    Email = data.Email,
                    RequireDate = data.RequireDate,
                    Age = data.Age,
                    Location = data.Location
                });

                _logger.LogInformation("a message has been received");
            }
        }
    }
}
