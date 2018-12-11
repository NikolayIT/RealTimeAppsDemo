namespace WebSocketsDemo
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using SharedLibrary;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets(
                new WebSocketOptions
                    {
                        KeepAliveInterval = TimeSpan.FromSeconds(120),
                        ReceiveBufferSize = 4 * 1024,
                    });
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
