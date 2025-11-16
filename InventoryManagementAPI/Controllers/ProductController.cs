using InventoryManagementAPI.Models;
using InventoryManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("buscarProdutos")]
        public ActionResult GetAllProducts(string? name, int page = 1, int pageSize = 50)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Os parâmetros 'page' e 'pageSize' devem ser maiores que zero");

                var result = _productService.GetProducts(name, page, pageSize);
                var products = result.Products; // Acessa a lista de produtos
                var totalCount = result.TotalCount; // Acessa o total de produtos

                if (!products.Any())
                    return NotFound("Nenhum produto encontrado");

                return Ok(new { products, totalCount });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar produtos: {ex.Message}");
            }
        }



        [HttpPost]
        [Route("inserir")]
        public ActionResult AddProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest("O produto não pode ser nulo");

                if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.Quantity < 0)
                    return BadRequest("Todos os campos obrigatórios devem ser preenchidos corretamente"); //Melhorar mensagem

                var adicionado = _productService.AddProduct(product);

                if (!adicionado)
                    return BadRequest("Falha ao adicionar produto");

                return Ok("Produto adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao adicionar o produto: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("atualizar/{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if (product == null || product.Id != id)
                    return BadRequest("Produto inválido ou ID inconsistente");

                var atualizado = _productService.UpdateProduct(id, product);

                if (!atualizado)
                    return NotFound("Falha ao atualizar produto");

                return Ok("Produto atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o produto: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("deletar/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var deletado = _productService.DeleteProduct(id);

                if (!deletado)
                    return NotFound("Produto não encontrado para exclusão");

                return Ok("Produto deletado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o produto: {ex.Message}");
            }
        }
    }
}
