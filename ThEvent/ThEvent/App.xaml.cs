﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using ThEvent.Data;
using ThEvent.Models;
using System.IO;

namespace ThEvent
{
    public partial class App : Application
    {
        public const int ANONYM_ID = -1;
        public static int UserId = ANONYM_ID;

        static ThEventDatabase database;
        public static ThEventDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ThEventDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ThEvent.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            var ini = new Initializer();
            ini.Init();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
