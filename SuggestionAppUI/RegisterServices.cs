﻿using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace SuggestionAppUI;

public static class RegisterServices
{
	public static void ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddRazorPages();
		builder.Services.AddMemoryCache();
		builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
		builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

		builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
		builder.Services.AddAuthorization(options =>
		{
			options.AddPolicy("Admin", policy =>
			{
				policy.RequireClaim("jobTitle", "Admin");
			});
		});
		builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
		{
			options.ResponseType = OpenIdConnectResponseType.Code;
			options.Scope.Add(options.ClientId);
		});

		builder.Services.AddSingleton<IDbConnection, DbConnection>();
		builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
		builder.Services.AddSingleton<IStatusData, MongoStatusData>();
		builder.Services.AddSingleton<ISuggestionData, MongoSuggestionData>();
		builder.Services.AddSingleton<IUserData, MongoUserData>();
	}
}
