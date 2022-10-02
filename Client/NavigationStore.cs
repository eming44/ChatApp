using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class NavigationStore
    {
        private ViewModelBase currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => this.currentViewModel;
            set
            {
                if (this.currentViewModel != value)
                {
                    this.currentViewModel = value;
                }
            }
        }
    }
}
