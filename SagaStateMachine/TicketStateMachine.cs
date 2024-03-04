using Events.SendEmailEvents;
using Events.TicketEvents;
using MassTransit;

namespace SagaStateMachine {
    public class TicketStateMachine : MassTransitStateMachine<TicketStateData> {
        // 4 states
        public State AddTicket { get; private set; }
        public State CancelTicket { get; private set; }
        public State CancelSendEmail { get; private set; }
        public State SendEmail { get; private set; }

        // 4 events

        public Event<IAddTicketEvent> AddTicketEvent { get; private set; }
        public Event<ICancelGenerateTicketEvent> CancelGenerateTicketEvent { get; private set; }
        public Event<ICancelSendEmailEvent> CancelSendEmailEvent { get; private set; }
        public Event<ISendEmailEvent> SendEmailEvent { get; private set; }

        public TicketStateMachine() {
            InstanceState(s => s.CurrentState);
            Event(() => AddTicketEvent, a => a.CorrelateById(m => m.Message.TicketId));
            Event(() => CancelGenerateTicketEvent, a => a.CorrelateById(m => m.Message.TicketId));
            Event(() => CancelSendEmailEvent, a => a.CorrelateById(m => m.Message.TicketId));
            Event(() => SendEmailEvent, a => a.CorrelateById(m => m.Message.TicketId));

            Initially(
                When(AddTicketEvent).Then(context => {
                    context.Saga.TicketId = context.Message.TicketId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.TicketNumber = context.Message.TicketNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(AddTicket).Publish(context => new GenerateTicketEvent(context.Saga)));

            During(AddTicket,
                When(SendEmailEvent)
                .Then(context => {
                    context.Saga.TicketId = context.Message.TicketId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.TicketNumber = context.Message.TicketNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(SendEmail));

            During(AddTicket,
                When(CancelGenerateTicketEvent)
                .Then(context => {
                    context.Saga.TicketId = context.Message.TicketId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.TicketNumber = context.Message.TicketNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(CancelTicket));

            During(SendEmail,
                When(CancelSendEmailEvent)
                .Then(context => {
                    context.Saga.TicketId = context.Message.TicketId;
                    context.Saga.Title = context.Message.Title;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.TicketNumber = context.Message.TicketNumber;
                    context.Saga.Age = context.Message.Age;
                    context.Saga.Location = context.Message.Location;
                }).TransitionTo(CancelSendEmail));
        }
    }
}
