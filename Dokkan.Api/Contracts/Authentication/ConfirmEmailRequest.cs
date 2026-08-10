namespace Dokkan.Api.Contracts.Authentication;

public record ConfirmEmailRequest
(string UserId,
    string Code);
