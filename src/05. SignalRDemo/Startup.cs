namespace SignalRDemo
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using SharedLibrary;

    using SignalRDemo.Hubs;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR().AddMessagePackProtocol();
            services.AddSingleton<IOrderService, OrderService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSignalR(routes => routes.MapHub<CoffeeHub>("/coffeehub"));
            app.UseMvc();
        }
    }
}
