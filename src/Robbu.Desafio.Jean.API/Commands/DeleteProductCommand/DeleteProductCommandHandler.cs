using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using MediatR;

namespace Robbu.Desafio.Jean.API.Commands.DeleteProductCommand
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommandResult>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
            {
                return CommandResult.NotFound();
            }

            product.IsDeleted = true;

            _repository.Update(product);
            await _unitOfWork.CommitAsync();

            return CommandResult.Success();
        }
    }
}