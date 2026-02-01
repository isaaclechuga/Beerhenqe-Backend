using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MVCconCapasGraphQL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class GraphQLController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _documentExecuter = documentExecuter ?? throw new ArgumentNullException(nameof(documentExecuter));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JObject body)
        {
            if (body == null)
                return BadRequest(new { code = 400, message = "Body vacío o inválido." });

            var query = body.Value<string>("query");
            var operationName = body.Value<string>("operationName");
            var variables = body["variables"] as JObject;
            var inputs = variables?.ToInputs();

            var result = await _documentExecuter.ExecuteAsync(new ExecutionOptions
            {
                Schema = _schema,
                Query = query,
                OperationName = operationName,
                Inputs = inputs
            });

            if (result.Errors?.Count > 0)
                return StatusCode(500, new { errors = result.Errors });

            return Ok(result.Data);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get() => Ok(new { message = "GraphQL API activa. Usa POST /graphql con tu consulta o mutación." });
    }

}
