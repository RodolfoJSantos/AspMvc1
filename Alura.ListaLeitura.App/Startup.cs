﻿using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection service)
		{
			service.AddRouting();
		}
		public void Configure(IApplicationBuilder app)
		{
			var builder = new RouteBuilder(app);
			builder.MapRoute("Livros/ParaLer", LivrosParaLer);
			builder.MapRoute("Livros/Lendo", LivrosLendo);
			builder.MapRoute("Livros/Lidos", LivrosLidos);
			var rotas = builder.Build();

			app.UseRouter(rotas);

			//app.Run(Roteamento);
		}

		public Task Roteamento(HttpContext httpContext)
		{
			var _repo = new LivroRepositorioCSV();
			var caminhosAtendidos = new Dictionary<string, RequestDelegate>
			{
				{"/Livros/ParaLer", LivrosParaLer},
				{"/Livros/Lendo", LivrosLendo},
				{"/Livros/Lidos", LivrosLidos}
			};

			if (caminhosAtendidos.ContainsKey(httpContext.Request.Path))
			{
				var metodo = caminhosAtendidos[httpContext.Request.Path];
				return metodo.Invoke(httpContext);
			}

			httpContext.Response.StatusCode = 404;
			return httpContext.Response.WriteAsync("Caminho inexistente.");
		}

		public Task LivrosParaLer(HttpContext httpContext)
		{
			var _repo = new LivroRepositorioCSV();
			return httpContext.Response.WriteAsync(_repo.ParaLer.ToString());
		}

		public Task LivrosLendo(HttpContext httpContext)
		{
			var _repo = new LivroRepositorioCSV();
			return httpContext.Response.WriteAsync(_repo.Lendo.ToString());
		}

		public Task LivrosLidos(HttpContext httpContext)
		{
			var _repo = new LivroRepositorioCSV();
			return httpContext.Response.WriteAsync(_repo.Lidos.ToString());
		}
	
	}
}