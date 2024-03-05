using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TeamChat.Database;
using TeamChat.Hubs;
using TeamChat.MiddleWares;
using TeamChat.Repositories.UnitOfWork;
using TeamChat.Services.ChannelService;
using TeamChat.Services.ConversationService;
using TeamChat.Services.DirectMessageService;
using TeamChat.Services.MemberService;
using TeamChat.Services.MessageService;
using TeamChat.Services.ProfileService;
using TeamChat.Services.ServerService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IServerService, ServerService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IDirectMessageService, DirectMessageService>();
builder.Services.AddScoped<IConversationService, ConversationService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder =>
            builder
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true)
                .AllowAnyHeader()
    );
});

builder
    .Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            .Json
            .ReferenceLoopHandling
            .Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.MapControllers();

app.UseMiddleware<JwtMiddleware>();

app.UseEndpoints(endpoint => endpoint.MapHub<ChatMessageHub>("api/chat/message"));
app.UseEndpoints(endpoint => endpoint.MapHub<ChatDirectMessageHub>("api/chat/directMessage"));

app.Run();
