using BuildingBlock.Domain;
using MediatR;

namespace BuildingBlock.Application.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
