using Notebook.Models;
using Notebook.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Notebook.ViewModels
{
    [QueryProperty(nameof(ParentId), nameof(ParentId))]
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public Command AddChildCommand { get; }

        public Command<Item> ItemTapped { get; }
        public Command<Item> BreadcrumbTapped { get; }

        public Command<Item> ViewInfoCommand { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            AddChildCommand = new Command(OnAddChild);
            BreadcrumbTapped = new Command<Item>(BreadcrumbSelected);
            ViewInfoCommand = new Command<Item>(ViewInfo);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true, this.ParentId);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public ObservableCollection<Item> SelectedItems { get; set; }

        public string ParentId { get; set; }

        private async void OnAddItem(object obj)
        {
            if (string.IsNullOrEmpty(this.ParentId))
                await Shell.Current.GoToAsync(nameof(NewItemPage));
            else
                await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}={this.ParentId}");
        }

        private async void OnAddChild(object obj)
        {
            if (obj is Item item)
            {
                await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}={item?.Id}");
            }
        }

        private async void ViewInfo(Item item)
        {
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item?.Id}");
        }

        void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            SelectedItems.Add(item);
            this.ParentId = item.Id;
            ExecuteLoadItemsCommand();
        }

        void BreadcrumbSelected(Item item)
        {
            if (item == null)
                return;

            while (SelectedItems.Count > 0 && SelectedItems[SelectedItems.Count - 1].Id != item.Id)
            {
                SelectedItems.Remove(SelectedItems[SelectedItems.Count - 1]);
            }

            this.UpdateNavigation();
        }

        public bool OnBackButtonPressed()
        {
            if (SelectedItems.Count == 0)
                return false;

            SelectedItems.Remove(SelectedItems[SelectedItems.Count - 1]);

            this.UpdateNavigation();

            return true;
        }

        private void UpdateNavigation()
        {
            if (SelectedItems.Count > 0)
                this.ParentId = SelectedItems[SelectedItems.Count - 1].Id;
            else
                this.ParentId = null;

            ExecuteLoadItemsCommand();
        }
    }
}