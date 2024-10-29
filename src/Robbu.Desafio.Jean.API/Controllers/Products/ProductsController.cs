using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Robbu.Desafio.Jean.API.Commands.CreateProductCommand;
using Robbu.Desafio.Jean.API.Commands.DeleteProductCommand;
using Robbu.Desafio.Jean.API.Commands.UpdateProductCommand;
using Robbu.Desafio.Jean.API.Models.Responses;
using Robbu.Desafio.Jean.API.Persistence.Entities;
using Robbu.Desafio.Jean.API.Queries.GetAllProductsQuery;
using Robbu.Desafio.Jean.API.Queries.GetProductByIdQuery;
using Robbu.Desafio.Jean.API.Queries.GetProductCountQuery;
using System.ComponentModel.DataAnnotations;

namespace Robbu.Desafio.Jean.API.Controllers.Products
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, IMemoryCache cache, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] int? page, [FromQuery] int? limit)
        {
            _logger.LogInformation($"page {page} limit {limit}");

            var cachedResponse = await _cache.GetOrCreateAsync($"ProductsListPage{page}Limit{limit}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                entry.SetPriority(CacheItemPriority.High);

                var queryAllProducts = new GetAllProductsQuery(limit, page);

                var allProducts = await _mediator.Send(queryAllProducts);

                if (allProducts == null || !allProducts.Any())
                {
                    return null;
                }

                var queryCountProducts = new GetProductCountQuery();
                var countProducts = await _mediator.Send(queryCountProducts);

                var pagination = new PaginationResponse(countProducts, limit);
                return new ApiPaginatedResponse<IEnumerable<Product>>("Listagem de produtos realizada com sucesso", allProducts, pagination);
            });

            if (cachedResponse == null)
            {
                return NoContent();
            }

            return Ok(cachedResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute, Required] int id)
        {
            var command = new GetProductByIdQuery(id);

            var product = await _mediator.Send(command);

            // É possível também retornar um NoContent, dependendo da política de boas práticas
            if (product == null)
            {
                return NotFound();
            }

            var response = new ApiResponse<Product?>("Produto encontrado com sucesso", product);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody, Required] CreateProductCommand command)
        {
            var product = await _mediator.Send(command);
            var response = new ApiResponse<Product?>("Produto cadastrado com sucesso", product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody, Required] UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsNotFound)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute, Required] int id)
        {
            var command = new DeleteProductCommand(id);

            var result = await _mediator.Send(command);
            if (result.IsNotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}