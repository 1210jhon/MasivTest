using Microsoft.AspNetCore.Http;
using System;

namespace Rest.API.Infrastructure.Services.IdentityServices
{
    public class IdentityServices : IIdentityServices
    {
        #region Variables

        private readonly IHttpContextAccessor _context;

        #endregion

        #region Builder

        public IdentityServices(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Methods

        public string GetUserIdentity()
        {
            var auth = _context.HttpContext.User.Identity.IsAuthenticated;
            if (auth)
            {
                return _context.HttpContext.User.FindFirst("sub").Value;
            }
            return "NoAuth";
        }

        #endregion
    }
}
