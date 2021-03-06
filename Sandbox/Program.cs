﻿using System;
using Webmilio.Commons.DependencyInjection;
using WebmilioCommons.Sandbox.Networking;

namespace WebmilioCommons.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceProvider();

            services
                .AddSingleton(s => new IntService(5))
                .AddSingleton<SimpleService>()
                .AddSingleton<TestClassY>()
                .AddSingleton<NetworkingTest>();

            var test = services.GetService<TestClassY>();
            test.Show();

            services.GetService<NetworkingTest>().Run();

            while (true)
            {
                services.Make<NotAService>().Show();
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }

    internal class SimpleService
    {
        public override string ToString()
        {
            return $"{base.ToString()}#{GetHashCode():X}";
        }
    }

    internal class IntService : ID
    {
        public IntService(int x)
        {
            X = x;
        }


        public int X { get; }
    }

    internal class TestClassY
    {
        private readonly SimpleService _simpleService;
        private readonly IC _intService;


        public TestClassY(SimpleService simpleService)
        {
            _simpleService = simpleService;
        }

        public TestClassY(SimpleService simpleService, IC intService)
        {
            _simpleService = simpleService;
            _intService = intService;
        }

        public TestClassY(SimpleService simpleService, IntService intService, int x)
        {
            _simpleService = simpleService;
            _intService = intService;
        }


        public void Show()
        {
            Console.WriteLine(_simpleService.ToString());
            Console.WriteLine(_intService.X);
        }
    }

    internal interface IIntProvider
    {
        int X { get; }
    }

    internal interface IA : IIntProvider
    {

    }
    internal interface IB : IA
    {

    }
    internal interface IC : IB
    {

    }
    internal interface ID : IC
    {

    }

    internal class NotAService
    {
        private IB _b;
        private BindedService _bs;
        private ScopedService _ss;

        public NotAService(IB b, BindedService bs, ScopedService ss)
        {
            _b = b;
            _bs = bs;
            _ss = ss;
        }

        public void Show()
        {
            Console.WriteLine("X: {0}; BS: {1}; SS: {2}", _b.X, _bs.Y, _ss.GetHashCode());
        }
    }

    [Service]
    internal class BindedService
    {
        public int Y { get; } = 10;
    }

    [Service(ServiceType.Scoped)]
    internal class ScopedService
    {
        
    }
}
