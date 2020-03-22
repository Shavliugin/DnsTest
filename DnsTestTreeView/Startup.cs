using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DnsTestTreeView.DataBaseLogic;
using DnsTestTreeView.BuisnessLogic;
using DnsTestTreeView.DataBaseLogic.Interfaces;

namespace DnsTestTreeView
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddScoped<ITreeNodeManager, TreeNodeManager>();
            services.AddScoped<ITreeNodeDbFiller, MSSQLFiller>();
            services.AddScoped<ITreeNodesProvider, TreeNodesMSSQLProvider>();            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITreeNodeDbFiller filler)
        {
            //Было нужно для первоначального заполнения БД
            //filler.FillDb();                        

            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
