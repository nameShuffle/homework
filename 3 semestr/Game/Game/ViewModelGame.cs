using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Game
{
    class ViewModelGame : INotifyPropertyChanged
    {
        public ViewModelGame()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}