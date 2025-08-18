using Domain.Entities;
using MediatR;

namespace Application.Mediator.DocumentTypes.Queries
{
    public class GetDocumentTypesQuery : IRequest<IEnumerable<DocumentType>>
    {

    }
}
