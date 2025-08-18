using Application.Common.Interfaces;
using Application.Mediator.DocumentTypes.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Mediator.DocumentTypes.QueryHandler
{
    public class GetDocumentTypesHandler : IRequestHandler<GetDocumentTypesQuery, IEnumerable<DocumentType>>
    {
        private readonly IGenericRepository<DocumentType> _documentTypeRepo;

        public GetDocumentTypesHandler(IGenericRepository<DocumentType> repository)
        {
            _documentTypeRepo = repository;
        }

        public async Task<IEnumerable<DocumentType>> Handle(GetDocumentTypesQuery request, CancellationToken cancellationToken)
        {
            var documentTypes = await _documentTypeRepo.GetAllAsync(cancellationToken);
            return documentTypes ?? new List<DocumentType>();
        }
    }
}
