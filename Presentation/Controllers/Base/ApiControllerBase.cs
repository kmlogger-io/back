using Domain.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common
{
    /// <summary>
    /// Controller base com respostas padrão documentadas para Swagger.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status500InternalServerError)]
    public abstract class ApiControllerBase : ControllerBase;
}
