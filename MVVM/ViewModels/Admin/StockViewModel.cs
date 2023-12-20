using Cashbox.Core;
using Cashbox.Core.Commands;
using Cashbox.MVVM.Models;
using Cashbox.MVVM.ViewModels.Data;
using Cashbox.MVVM.Views.Pages.Admin;
using Cashbox.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashbox.MVVM.ViewModels.Admin
{
    public class StockViewModel : ViewModelBase
    {
        #region Props

        private string _newCategoryTitle = string.Empty;
        public string NewCategoryTitle
        {
            get => _newCategoryTitle;
            set => Set(ref _newCategoryTitle, value);
        }

        public ObservableCollection<ProductCategoryViewModel> CollectionProductCategories { get; } = []; 

        #endregion


        #region Command

        public RelayCommand AddCategoryCommand { get; set; }
        private bool CanAddCategoryCommandExecute(object p)
        {
            if (NewCategoryTitle == null)
                return false;
            return true;
            
        }
        private async void OnAddCategoryCommandExecuted(object p)
        {
            var data = await ProductCategoryViewModel.CreateProductCategory(NewCategoryTitle);
            if (data != null) CollectionProductCategories.Add(data);
        }

        #endregion

        public override async void OnLoad()
        {
            CollectionProductCategories.Clear();
            var Categories = await ProductCategoryViewModel.GetProductCategory();
            foreach (var item in Categories) { CollectionProductCategories.Add(item); }
        }

        public StockViewModel()
        {
            AddCategoryCommand = new RelayCommand(OnAddCategoryCommandExecuted, CanAddCategoryCommandExecute);
        }
    }
}
