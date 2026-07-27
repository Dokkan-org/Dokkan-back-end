using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Dokkan.Api.Abstractions;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("ToProblem() called on a successful result.");

        var statusCode = result.Error.StatusCode??StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Status=statusCode,
            Detail=result.Error.Description

        };

        problemDetails.Extensions["code"] = result.Error.Code;

        return new ObjectResult(problemDetails) { StatusCode=statusCode};
    }
}

