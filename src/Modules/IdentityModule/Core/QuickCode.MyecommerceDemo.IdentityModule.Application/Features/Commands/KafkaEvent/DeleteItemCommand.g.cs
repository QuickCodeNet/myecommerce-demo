using System;
using System.Linq;
using QuickCode.MyecommerceDemo.Common.Mediator;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.KafkaEvent;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.KafkaEvent
{
    public class DeleteItemKafkaEventCommand : IRequest<Response<bool>>
    {
        public string TopicName { get; set; }

        public DeleteItemKafkaEventCommand(string topicName)
        {
            this.TopicName = topicName;
        }

        public class DeleteItemKafkaEventHandler : IRequestHandler<DeleteItemKafkaEventCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemKafkaEventHandler> _logger;
            private readonly IKafkaEventRepository _repository;
            public DeleteItemKafkaEventHandler(ILogger<DeleteItemKafkaEventHandler> logger, IKafkaEventRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemKafkaEventCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.TopicName);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}