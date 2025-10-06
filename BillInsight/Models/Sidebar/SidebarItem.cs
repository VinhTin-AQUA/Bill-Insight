using System.Reactive;
using ReactiveUI;

namespace BillInsight.Models.Sidebar
{
    public class SidebarItem
    {
        public string Name { get; set; } = string.Empty;
        public string Icon  { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit>? NavigateCommand { get; set; }
    }
}