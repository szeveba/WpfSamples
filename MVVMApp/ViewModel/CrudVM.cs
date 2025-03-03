using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MVVMApp.Model;

namespace MVVMApp.ViewModel
{
    internal partial class CrudVM : ObservableRecipient
    {

        private Context ctx;
        public CrudVM()
        {
            ctx = new Context();
            VisibleItems = [.. ctx.TodoItems];
            //VisibleItems = new ObservableCollection<TodoItem>();
            //foreach (var item in ctx.TodoItems)
            //{
            //    VisibleItems.Add(item);
            //}
            this.PropertyChanged += CrudVM_PropertyChanged;
            this.PropertyChanging += CrudVM_PropertyChanging; ;
        }

        private void CrudVM_PropertyChanging(object? sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem) && SelectedItem != null)
            {
                SelectedItem.PropertyChanged -= SelectedItem_PropertyChanged;
            }
        }

        private void CrudVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedItem):
                    if (SelectedItem != null) SelectedItem.PropertyChanged += SelectedItem_PropertyChanged;
                    break;
                case nameof(IsFiltered):
                case nameof(SearchPhrase):
                    var q = ctx.TodoItems.AsEnumerable();
                    if (!string.IsNullOrWhiteSpace(SearchPhrase))
                    {
                        var sw = SearchPhrase.ToLower();
                        q = q.Where(x => x.Title.Contains(sw));
                    }

                    if (IsFiltered)
                    {
                        q = q.Where(x => x.IsDone == false);
                    }

                    q = q.OrderByDescending(x => x.DeadLine.HasValue).ThenBy(x => x.Title);
                    VisibleItems.Clear();
                    foreach (var item in q)
                    {
                        VisibleItems.Add(item);
                    }
                    break;
            }
        }

        private void SelectedItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (ctx.Entry(SelectedItem).State)
            {
                case EntityState.Modified:
                    ListEnabled = false;
                    break;
                default:
                    ListEnabled = true;
                    break;
            }
        }

        [ObservableProperty] private bool _listEnabled = true;
        [ObservableProperty] private bool _isFiltered;
        [ObservableProperty] private string? _searchPhrase;
        [ObservableProperty] private TodoItem? _selectedItem;
        public ObservableCollection<TodoItem> VisibleItems { get; set; }

        [RelayCommand]
        private void Create()
        {
            if (SelectedItem != null)
            {
                ctx.Entry(SelectedItem).State = EntityState.Unchanged;
            }
            SelectedItem = new TodoItem();
        }

        [RelayCommand]
        private void Save()
        {
            if (SelectedItem!.Id == 0)
            {
                ctx.Add(SelectedItem);
                VisibleItems.Add(SelectedItem);
            }

            try
            {
                ctx.SaveChanges();
                ListEnabled = true;
            }
            catch (Exception e)
            {
                if (SelectedItem.Id == 0)
                {
                    VisibleItems.Remove(SelectedItem);
                }

                ShowError(e);
            }
        }

        private void ShowError(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedItem != null)
            {
                var result = MessageBox.Show("Are you sure?", "Delete confirmation", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    ctx.Remove(SelectedItem);
                    try
                    {
                        ctx.SaveChanges();
                        VisibleItems.Remove(SelectedItem);
                    }
                    catch (Exception e)
                    {
                        ShowError(e);
                    }
                }

            }
        }
    }
}
