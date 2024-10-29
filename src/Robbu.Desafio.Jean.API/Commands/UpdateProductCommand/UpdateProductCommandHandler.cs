using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using MediatR;

namespace Robbu.Desafio.Jean.API.Commands.UpdateProductCommand
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, CommandResult>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
            {
                return CommandResult.NotFound();
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price.Value;
            product.Date = request.Date.Value;

            _repository.Update(product);
            await _unitOfWork.CommitAsync();

            return CommandResult.Success();
        }
    }
}