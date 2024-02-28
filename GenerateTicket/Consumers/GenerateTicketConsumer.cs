using AutoMapper;
using Events.SendEmailEvents;
using Events.TicketEvents;
using GenerateTicket.Common;
using GenerateTicket.Models;
using GenerateTicket.Services;
using MassTransit;

namespace GenerateTicket.Consumers {
    public class GenerateTicketConsumer : IConsumer<IGenerateTicketEvent> {
        // This consumer is listening to the IGenerateTicketEvent
        // But, Ticket Service publish its message to the IAddTicketEvent
        // Here State machine will transform IAddTicketEvent to the IGenerateTicketEvent 
        private readonly ITicketInfoService _ticketInfoService;
        private readonly ILogger<GenerateTicketConsumer> _logger;
        private readonly IMapper _mapper;

        public GenerateTicketConsumer(ITicketInfoService ticketInfoService, ILogger<GenerateTicketConsumer> logger, IMapper mapper) {
            _ticketInfoService = ticketInfoService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IGenerateTicketEvent> context) {
            var data = context.Message;

            if (data is not null) {
                // Check if Age is more 17 and less 80, then publish ISendEmailEvent
                if (data.Age > 17 && data.Age < 80) {
                    var mapModel = _mapper.Map<TicketInfo>(data);

                    var res = await _ticketInfoService.AddTicketInfo(mapModel);
                    if (res is not null) {
                        await context.Publish<ISendEmailEvent>(new {
                            TicketId = data.TicketId,
                            Title = data.Title,
                            Email = data.Email,
                            RequireDate = data.RequireDate,
                            Age = data.Age,
                            Location = data.Location,
                            TicketNumber = res.TicketNumber
                        });
                        _logger.LogInformation($"Message sent == TicketId is {data.TicketId}");
                    }
                }
                else {
                    // Publish cancel transaction event
                    await context.Publish<ICancelGenerateTicketEvent>(new {
                        TicketId = data.TicketId,
                        Title = data.Title,
                        Email = data.Email,
                        RequireDate = data.RequireDate,
                        Age = data.Age,
                        Location = data.Location
                    });
                    _logger.LogInformation($"Message canceled== TicketId is {data.TicketId}");
                }
            }
        }
    }
}
