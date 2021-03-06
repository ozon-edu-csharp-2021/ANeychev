using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Grpc;
using OzonEdu.MerchandiseService.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchandiseService.Infrastructure.Queries.EmployeeAggregate;

namespace OzonEdu.MerchandiseService.Controllers.V1
{
    /// <summary>
    ///     Сервис учёта выдачи мерча сотрудникам
    /// </summary>
    [ApiController]
    [Route("v1/merchandises/[controller]/[action]")]
    [Produces("application/json")]
    public sealed class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="mediator"></param>
        public MerchandiseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Запрос на выдачу мерча
        /// </summary>
        /// <param name="employeeId">идентификатор работника</param>
        /// <param name="merchPackTypeId">идентификатор набора мерча</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{employeeId:long}/{merchPackTypeId:int}")]
        public async Task<ActionResult<int>> GetMerchPack(long employeeId, int merchPackTypeId,
            CancellationToken cancellationToken = default)
        {
            var query = new CreateMerchRequestCommand
            {
                EmployeeId = employeeId,
                MerchPackTypeId = merchPackTypeId
            };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        ///     Возвращает информацию о выдаче мерча
        /// </summary>
        /// <param name="employeeId">идентификатор работника</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        [HttpGet("{employeeId:long}")]
        public async Task<ActionResult<GetInfoResponse>> GetInfoAboutMerchPacks(long employeeId,
            CancellationToken cancellationToken = default)
        {
            var getInfoAboutGiveOutMerchPacksForEmployeeQuery = new GetInfoAboutGiveOutMerchPacksForEmployeeQuery
            {
                EmployeeId = employeeId
            };
            var result = await _mediator.Send(getInfoAboutGiveOutMerchPacksForEmployeeQuery, cancellationToken);

            return new GetInfoResponse
            {
                Items =
                {
                    result.Select(x => new MerchItemResponseUnit
                    {
                        ItemId = x.Sku.Value,
                        ItemName = x.Name.Value,
                        Quantity = x.Quantity.Value
                    })
                }
            };
        }
    }
}