using CarSharingApp.Application.Contracts.Authorization;

namespace CarSharingApp.Web.Clients.Interfaces
{
    public interface IAuthorizationServicePublicApiClient
    {
        Task<HttpResponseMessage> TryAuthorize(AuthorizationRequest request);
    }
}
