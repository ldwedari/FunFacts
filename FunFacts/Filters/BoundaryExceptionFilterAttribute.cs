using System;
using System.Web.Http.Filters;

namespace FunFacts.Filters
{
    /// <summary>
    /// Strip the message and the callstack from the exception before sending it to the client.
    /// </summary>
    public class BoundaryExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        { 
            // Eventually log the exception.
            throw new Exception("Contact technical support");
        }
    }
}