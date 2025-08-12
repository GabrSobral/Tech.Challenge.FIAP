using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Application.Services.Administrativo.Produtos.AdicionarProduto;
using Tech.Challenge.Application.Services.Administrativo.Produtos.AtualizarProduto;
using Tech.Challenge.Application.Services.Administrativo.Produtos.DeletarProduto;
using Tech.Challenge.Application.Services.Administrativo.Produtos.ListarProdutos;

namespace Tech.Challenge.Presentation.Http;

[Authorize]
[Route("produtos")]
[ApiController]
public class ProdutoController(
    IHttpContextAccessor HttpContextAccessor,
    AdicionarProdutoService AdicionarProdutoService,
    DeletarProdutoService DeletarProdutoService,
    ListarProdutosService ListarProdutosService,
    AtualizarProdutoService AtualizarProdutoService) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> ListarProdutos(
        [FromQuery] int? page,
        [FromQuery] int? take,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Produtos.ListarProdutos.Request(page, take);
            var response = await ListarProdutosService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            var request = new Application.Services.Administrativo.Produtos.ListarProdutos.Request(page, take);
            var response = await ListarProdutosService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> AdicionarProduto(
        [FromBody] Application.Services.Administrativo.Produtos.AdicionarProduto.Request body,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await AdicionarProdutoService.Execute(body, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("{produtoId}")]
    public async Task<IActionResult> DeletarProduto(
        [FromRoute] Guid produtoId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Produtos.DeletarProduto.Request(produtoId);
            var response = await DeletarProdutoService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarVeiculo(
        [FromRoute] Guid id,
        [FromBody] Application.Services.Administrativo.Produtos.AtualizarProduto.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Produtos.AtualizarProduto.Request(id, body.Nome, body.PrecoUnitario, body.Quantidade, body.Tipo, body.UnidadeMedida);
            var response = await AtualizarProdutoService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }
}
