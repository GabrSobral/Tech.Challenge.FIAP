using System.Net;
using System.Text.Json;
using Tech.Challenge.Domain.Exceptions;

namespace Tech.Challenge.Middlewares;

public sealed class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            logger.LogError(error, error.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            SwitchException(error, response);

            var result = JsonSerializer.Serialize(new
            {
                title = error.GetType().Name,
                status = response.StatusCode,
                occuredAt = DateTime.UtcNow,
                error = error.Message
            });

            NewRelic.Api.Agent.NewRelic.NoticeError(error);

            await response.WriteAsync(result);
        }
    }

    private void SwitchException(Exception error, HttpResponse response)
    {
        switch (error)
        {
            case UsuarioAlreadyExistsException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Usuário já cadastrado] {error.Message}");
                break;

            case ClienteCpfMismatchException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[CPF do cliente é invalido] {error.Message}");
                break;

            case ClienteNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogError($"[Cliente não foi encontrado] {error.Message}");
                break;

            case ClienteAlreadyRegisteredException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Cliente já cadastrado] {error.Message}");
                break;

            case ProductAlreadyExistsException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Produto já cadastrado] {error.Message}");
                break;

            case DomainError:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Erro de domínio] {error.Message}");
                break;

            case OrdemServicoNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogError($"[Ordem de Serviço não encontrada] {error.Message}");
                break;

            case OrdemServicoCantBeCanceledException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Ordem de Serviço não pode ser cancelada] {error.Message}");
                break;

            case ProductNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogError($"[Produto não encontrado] {error.Message}");
                break;

            case ServicoNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogError($"[Serviço não encontrado] {error.Message}");
                break;

            case VeiculoAlreadyRegisteredException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Veículo já cadastrado] {error.Message}");
                break;

            case VeiculoNotFoundException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogError($"[Veículo não encontrado] {error.Message}");
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError($"[Internal error request] {error.Message}");
                break;
        }
    }
}