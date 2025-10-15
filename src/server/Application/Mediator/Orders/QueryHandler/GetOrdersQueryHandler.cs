using Application.Common.Interfaces;
using Application.DTOs;
using Application.Mediator.Orders.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Mediator.Orders.QueryHandler
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(
            IGenericRepository<Order> orderRepo,
            IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var query = _orderRepo.GetAll()
                .Include(u => u.User)
                .Include(o => o.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(o => o.UserId == request.UserId)
                .OrderByDescending(o => o.CreatedDate);

            var orders = await _orderRepo.ToListAsync(query, cancellationToken);

            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
