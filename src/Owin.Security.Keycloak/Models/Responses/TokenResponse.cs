﻿using System.Collections.Specialized;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;

namespace Owin.Security.Keycloak.Models.Responses
{
    internal class TokenResponse : OidcResponse
    {
        public TokenResponse(string encodedJson)
            : this(JObject.Parse(encodedJson))
        {
        }

        public TokenResponse(JObject json)
        {
            var authResult = new NameValueCollection();

            // Convert JSON to NameValueCollection type
            foreach (var item in json)
                authResult.Add(item.Key, item.Value.ToString());

            InitFromRequest(authResult);
        }

        public TokenResponse(JsonWebToken accessToken, JsonWebToken idToken = null, JsonWebToken refreshToken = null)
        {
            IdToken = idToken;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string ExpiresIn { get; private set; }
        public string TokenType { get; private set; }

        public JsonWebToken IdToken { get; private set; }
        public JsonWebToken AccessToken { get; private set; }
        public JsonWebToken RefreshToken { get; private set; }

        protected new void InitFromRequest(NameValueCollection authResult)
        {
            base.InitFromRequest(authResult);

            ExpiresIn = authResult.Get(OpenIdConnectParameterNames.ExpiresIn);
            TokenType = authResult.Get(OpenIdConnectParameterNames.TokenType);

            IdToken = new JsonWebToken(authResult.Get(OpenIdConnectParameterNames.IdToken));
            AccessToken = new JsonWebToken(authResult.Get(OpenIdConnectParameterNames.AccessToken));
            RefreshToken = new JsonWebToken(authResult.Get(Constants.OpenIdConnectParameterNames.RefreshToken));
        }
    }
}