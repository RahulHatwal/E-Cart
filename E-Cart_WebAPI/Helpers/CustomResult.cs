using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace E_Cart_WebAPI.Helpers
{
    public class CustomResult : IActionResult
    {
        private readonly int _statusCode;
        private readonly object _value;
        private readonly string _message;
        private readonly bool _success;

        public CustomResult(int statusCode, object value, bool success, string message)
        {
            _statusCode = statusCode;
            _value = value;
            _success = success;
            _message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(new Dictionary<string, object>
            {
                
                { "message", _message },
                { "success", _success },
                { "data", _value ?? new object() }
            })
            {
                StatusCode = _statusCode,
                DeclaredType = typeof(object)
            };
            context.HttpContext.Response.Headers.Add("X-Custom-Header", "Custom Result");
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
