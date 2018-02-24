using MVVM_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication.ViewModels
{

    // NOT NEDDED? DELETE



    public class Layer_ViewModel : ObservableObject
    {
        private Layer_Model _layer;

        public Layer_ViewModel()
        {
            _layer = new Layer_Model();
        }

        public Layer_Model Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                RaisePropertyChanged();
                //RaisePropertyChanged("Guid");
                //RaisePropertyChanged("IsSelected");
            }
        }

        public string Name
        {
            get { return _layer.Name; }
            set
            {
                _layer.Name = value;
                RaisePropertyChanged();
            }
        }

        public Guid Id
        {
            get { return _layer.Id; }
            set
            {
                _layer.Id = value;
                RaisePropertyChanged();
            }
        }

        //public bool IsSelected
        //{
        //    get { return _layer.IsSelected; }
        //    set
        //    {
        //        _layer.IsSelected = value;
        //        RaisePropertyChanged();
        //    }
        //}
    }
}
