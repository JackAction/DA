using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MVVM_Framework
{
    // Die ursprüngliche Implementierung von ObservableObject kommt aus
    // Prism (Microsoft.Practices.Composite.Presentation)
    // http://stackoverflow.com/a/10093257/33311
    //
    // Dies ist eine Variante davon.
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Warnt den Compiler, falls es zum spezifizierten Namen kein Property im Objekt existiert.
        /// Diese Methode ist nur für Debug Build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Sicherstellen, dass der Proterty Name auf dem Objekt exisiert  
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }
    }
}
