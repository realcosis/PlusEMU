using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;
using Plus.Core;
using Plus.HabboHotel.Rooms.Chat.Commands;

namespace Plus;

public static class Program
{
    private const int MfBycommand = 0x00000000;
    public const int ScClose = 0xF060;

    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public static async Task Main(string[] args)
    {
        var collection = new ServiceCollection();
        collection.Scan(scan => scan.FromAssemblies(typeof(Program).Assembly)
            .AddClasses()
            .AsMatchingInterface()
            .WithScopedLifetime());
        collection.AddAssignableTo<IChatCommand>();

        var projectSolutionPath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationData(projectSolutionPath + "//Config//config.ini");
        collection.AddSingleton(configuration);

        var serviceProvider = collection.BuildServiceProvider();

        //XmlConfigurator.Configure();
        LogManager.LoadConfiguration("Config/nlog.config");
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;
        var currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += MyHandler;
        var environment = serviceProvider.GetRequiredService<IPlusEnvironment>();
        await environment.Start();
        while (true)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                Console.Write("plus> ");
                var input = Console.ReadLine();
                if (input.Length > 0)
                {
                    var s = input.Split(' ')[0];
                    ConsoleCommands.InvokeCommand(s);
                }
            }
        }
    }


    public static IServiceCollection AddAssignableTo<T>(this IServiceCollection services) =>
        services.Scan(scan => scan.FromAssemblies(typeof(Program).Assembly)
            .AddClasses(classes => classes.Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface))
            .As<T>().WithTransientLifetime());
    private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
    {
        var e = (Exception)args.ExceptionObject;
        //Logger.LogCriticalException("SYSTEM CRITICAL EXCEPTION: " + e);
        PlusEnvironment.PerformShutDown();
    }

    private enum CtrlType
    {
        CtrlCEvent = 0,
        CtrlBreakEvent = 1,
        CtrlCloseEvent = 2,
        CtrlLogoffEvent = 5,
        CtrlShutdownEvent = 6
    }

    private delegate bool EventHandler(CtrlType sig);
}