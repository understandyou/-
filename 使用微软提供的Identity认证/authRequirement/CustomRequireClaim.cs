using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace 使用微软提供的Identity认证.authRequirement
{
    public class CustomRequireClaim: IAuthorizationRequirement//需要的信息载体
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    public class CustomRequireHandler : AuthorizationHandler<CustomRequireClaim>//这个处理器需要注入到容器中
    {//handler决定有没有权限通过
        /// <summary>
        /// 决定是否授权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            //这个可以取出请求中的信息，用于判断是否授权
            //context.User.Claims
            //成功授权
            //context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}