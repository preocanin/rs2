using Microsoft.AspNetCore.Http;

using rs2.Models.Database;

namespace rs2.Models.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private class Payload
        {
            public int? UserId { get; set; } 
            public Role? Role { get; set; }
        };

        private IApplicationRepostitory AppRepo { get; set; }
        private IHttpContextAccessor Context { get; set; }

        public AuthRepository(IApplicationRepostitory repo, IHttpContextAccessor context)
        {
            AppRepo = repo;
            Context = context;

            if (Context.HttpContext.Request.Cookies.ContainsKey("access_token")
                && Context.HttpContext.Request.Cookies["access_token"] != ""
                && Context.HttpContext.Request.Cookies["access_token"] != "LOGED_OUT")
            {
                //TODO: validate token and set properties  
                bool status;
                string jsonPayload = AppRepo.ValidateToken(
                    Context.HttpContext.Request.Cookies["access_token"], out status);
                if(status)
                {
                    Payload payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Payload>(jsonPayload); 
                    if(payload.UserId != null && payload.Role != null)
                    {
                        CurrentUserId = payload.UserId.Value;
                        CurrentUserRole = payload.Role.Value;
                        IsValidToken = true;
                    }
                }
            }
        }

        public bool IsAuthenticated(Role role = Role.Client)
        {
            if (IsValidToken)
                return CurrentUserRole <= role;
            else
                return false;
        }

        public int CurrentUserId { get; set; } = -1;

        public Role CurrentUserRole { get; set; }

        private bool IsValidToken { get; set; } = false;
    }
}
