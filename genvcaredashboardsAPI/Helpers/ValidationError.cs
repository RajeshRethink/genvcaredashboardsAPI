using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Genworks.API.Helpers
{
    public class ValidationError
    {
        public static ProblemDetails PrepareErrorMessage(string msg, string path)
        {
            ProblemDetails problemDetails = new ProblemDetails();
            problemDetails.Detail = msg;
            problemDetails.Instance = path;
            problemDetails.Type = "https://httpstatuses.com/400";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "One or more validation errors occurred.";
            return problemDetails;
        }
    }
}
