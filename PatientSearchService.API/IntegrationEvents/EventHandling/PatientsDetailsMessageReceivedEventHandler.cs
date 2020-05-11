using iPas.Infrastructure.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace PatientSearchService.API
{
    public class PatientsDetailsMessageReceivedEventHandler :
        IIntegrationEventHandler<RegistrationReceivedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<PatientsDetailsMessageReceivedEventHandler> _logger;
        private IMediator _mediatr;
        private IRegistrationGRPCClientService _grpcRegistrationService;

        public PatientsDetailsMessageReceivedEventHandler(
                        IEventBus eventBus,
                        ILogger<PatientsDetailsMessageReceivedEventHandler> logger,
                        IMediator mediatr,
                        IRegistrationGRPCClientService registrationService
                        )
        {
            _eventBus = eventBus;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
            _grpcRegistrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
        }
        public async Task Handle(RegistrationReceivedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                Random rnd = new Random();
                Adt message = await _grpcRegistrationService.SearchRegistrationDataById(@event.DocumentId, @event.ClientId);
                int num = rnd.Next(1, 30);
                DateTime date;
                string[] format = { "yyyyMMdd" };
                if (message.content.PV1[0].admitDateTime != null) {

                    DateTime.TryParseExact(message.content.PV1[0].admitDateTime,
                                               format,
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               System.Globalization.DateTimeStyles.None,
                                               out date);
                }
                else
                {
                    string str = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'");
                    date = Convert.ToDateTime(str);
                }


                    var command = new CreateUpdatePatientDetailsInESCommand(message.content.PID[0].patientAccountNumber.id, date, "Payer" + "" + num.ToString(),
                                                                             message.content.PID[0].ssn, message.content.PV1[0].financialClass.financialClass,
                                                                             message.content.PV1[0].patientType, message.content.PID[0].internalId[0].id, message.content.PID[0].patientName[0].firstName,
                                                                             message.content.PID[0].patientName[0].lastName, message.content.PV1[0].admissionType,
                                                                             "RED-Flagged", "HAR"+ "" + num.ToString(), @event.ClientId.ToString(), @event.PatientVisitId.ToString(),
                                                                             message.content.EVN[0].operatorId.firstName, "DOS" + "" + num.ToString(), message.content.PID[0].sex);

                    _logger.LogInformation("-----Sending command: RunRegistrationRulesCommand");

                    await _mediatr.Publish(command);
                await Task.CompletedTask;
            }
        }
    }
}
