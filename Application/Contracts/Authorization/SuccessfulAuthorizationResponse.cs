﻿using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Authorization
{
    public record SuccessfulAuthorizationResponse(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("login")] string Login,
        [property: JsonPropertyName("email")] string Email
    );
}