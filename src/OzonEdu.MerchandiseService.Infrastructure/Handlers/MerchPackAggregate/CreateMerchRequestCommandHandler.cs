using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchandiseService.Infrastructure.Services.Interfaces;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.MerchPackAggregate
{
    public sealed class CreateMerchRequestCommandHandler : IRequestHandler<CreateMerchRequestCommand>
    {
        private readonly IApplicationService _applicationService;

        public CreateMerchRequestCommandHandler(IApplicationService applicationService) =>
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));

        /// <summary>
        ///     Запрос на выдачу сотруднику набора мерча
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Возвращает статус запроса</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Unit> Handle(CreateMerchRequestCommand request, CancellationToken cancellationToken = default)
        {
            var merchPack = await _applicationService.GetMerchPackAsync(request.MerchPackTypeId, cancellationToken);

            if (await _applicationService.CheckRepeatedMerchRequestAsync(request.EmployeeId, merchPack,
                cancellationToken))
                return Unit.Value;

            var merchRequest =
                await _applicationService.CreateMerchRequestAsync(request.EmployeeId, Email.Create(request.Email),
                    merchPack, cancellationToken);

            await _applicationService.TryGiveOutMerchRequestAsync(merchRequest, cancellationToken);

            return Unit.Value;
        }
    }
}