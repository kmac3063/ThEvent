﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThEvent.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThEvent
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {
        public List<String> tags;
        public DateTime date;
        public string titlePattern = null;
        public bool isPastEventsEnabled;

        public FilterPage()
        {
            tags = new List<string>();
            InitializeComponent();
            UpdateTags();          
        }

        private void UpdateTags()
        {
            List<Tag> tagListDB = App.Database.GetTagsAsync().Result;
            tagPicker.Items.Clear();
            foreach (Tag tag in tagListDB)
            {
                tagPicker.Items.Add(tag.Title);
            }
        }

        private void ChooseTag(object sender, EventArgs e)
        {
            if (tagPicker.SelectedIndex == -1) return;
            AddTag(tagPicker.Items[tagPicker.SelectedIndex]);
            tagPicker.SelectedIndex = -1;
        }

        private void AddTag(string text)
        {
            Label label = new Label();
            label.Text = text;
            label.BackgroundColor = Color.Gray;
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (sender_, e_) =>
            {
                if (await DisplayAlert("Подтвердите удаление", "Вы действительно хотите удалить этот тег?", "Да", "Нет"))
                {
                    tagList.Children.Remove(label);
                }
            };
            label.GestureRecognizers.Add(tap);
            tagList.Children.Add(label);
        }

        private void AddTag(object sender, EventArgs e)
        {
            Label findLabel = new Label();
            findLabel.Text = tagInput.Text;
            if (String.IsNullOrEmpty(tagInput.Text) || tagList.Children.Contains(findLabel)) return;
            AddTag(tagInput.Text);
            List<Tag> tagListDB = App.Database.GetTagsAsync().Result;
            if (tagListDB.Find(t => t.Title == tagInput.Text) == null)
            {
                Tag tag = new Tag() { Title = tagInput.Text };
                App.Database.SaveTagAsync(tag);
            }
            tagInput.Text = "";
            UpdateTags();
        }

        private void SaveFilterSettings(object sender, EventArgs e)
        {
            if (Date.IsEnabled) date = Date.Date;
            else date = new DateTime(0001, 1, 1);
            titlePattern = filterTitle.Text;
            foreach (Label label in tagList.Children) {
                tags.Add(label.Text);
            }
            isPastEventsEnabled = pastEvents.IsChecked;
            Navigation.PopAsync();
        }

        private void timeSelectorCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (timeSelector.IsChecked)
            {
                Date.IsEnabled = true;
                pastEventsStack.IsVisible = false;
            }
            else pastEventsStack.IsVisible = true;
        }
    }
}