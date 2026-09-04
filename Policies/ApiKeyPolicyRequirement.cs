using Microsoft.AspNetCore.Authorization;

namespace UGB.MVC.Aplicaciones.Seguras.Policies
{
    //para toda politica se necesita un IAuthorizationRequirement
    public class ApiKeyPolicyRequirement  : IAuthorizationRequirement
    {
        
    }
}