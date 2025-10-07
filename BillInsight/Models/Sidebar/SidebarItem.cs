using System.Reactive;
using ReactiveUI;

namespace BillInsight.Models.Sidebar
{
    public class SidebarItem : ReactiveObject
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set =>  this.RaiseAndSetIfChanged(ref _name, value);
            
        }
        
        private string _icon = "";

        public string Icon
        {
            get => _icon;
            set  => this.RaiseAndSetIfChanged(ref _icon, value);
        }
        
        private string _routeName = "";

        public string RouteName
        {
            get => _routeName;
            set  => this.RaiseAndSetIfChanged(ref _routeName, value);
        }
        
        private bool _isActive = false;

        public bool IsActive
        {
            get => _isActive;
            set => this.RaiseAndSetIfChanged(ref _isActive, value);
        }
    }
}