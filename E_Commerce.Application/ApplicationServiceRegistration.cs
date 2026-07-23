using E_Commerce.Application.Contracts;
using E_Commerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(c => { }, typeof(ApplicationServiceRegistration).Assembly);
            services.AddScoped<IProductService, ProductServices>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICasheService, CasheService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService,PaymentService>();
            return services;
        }
    }
}
