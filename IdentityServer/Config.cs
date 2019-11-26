using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        // Resources that will be used on our server
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var customProfile = new IdentityResource(
                name: "custom.profile",
                displayName: "Custom Profile",
                claimTypes: new[] { "customclaim" }
            );

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                customProfile
            };
        }

        // Apis (applications) that will be used on our server
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API"),
                new ApiResource("postman_api", "Postman Test Resource")
            };
        }


        // Clients (application) that will be able to access our identity server.
        public static IEnumerable<Client> GetClients()
        {
            // Credenciais da Aplicação
            return new List<Client>
            {

                 new Client
                {
                    ClientId = "postman-api",
                    ClientName = "Postman Test Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris =           { "https://www.getpostman.com/oauth2/callback" },
                    //NOTE: This link needs to match the link from the presentation layer - oidc-client
                    //      otherwise IdentityServer won't display the link to go back to the site
                    PostLogoutRedirectUris = { "https://www.getpostman.com" },
                    AllowedCorsOrigins =     { "https://www.getpostman.com" },
                    EnableLocalLogin = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "postman_api"
                    },
                    ClientSecrets = new List<Secret>() { new Secret("SomeValue".Sha256()) }
                },

                // OpenID Connect
                new Client
                {
                    // O Nome ÚNICO da nossa aplicação autorizada no nosso servidor de autoridade
                    ClientId = "iMastersApp",
                    
                    // Nome de exibição da nossa aplicação
                    ClientName = "iMasters Application",
                    
                    //Tipo de autenticação permitida
                    AllowedGrantTypes = GrantTypes.Implicit,

                    //Url de redicionamento para quando o login for efetuado com sucesso.
                    RedirectUris = { "https://localhost:44361/signin-oidc" },

                    //Url de redirecionamento para quando o logout for efetuado com sucesso.
                    PostLogoutRedirectUris = { "https://localhost:44361/signout-callback-oidc" },

                   AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "custom.profile"
                 }
                }
            };
        }

        // TestUser is an example class of the own IdentityServer, where we set login / password and display claims.
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "portal",
                    Password = "pass",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Portal"),
                        new Claim("website", "https://portal.com.br"),
                        new Claim("email", "contact@portal.com"),
                        new Claim("customclaim", "Minha claim customizada")
                    }

                },
                                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }

            };
        }
    }
}
