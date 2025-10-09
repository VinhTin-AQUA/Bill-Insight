using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Models.Invoices;
using ReactiveUI;

namespace BillInsight.ViewModels
{
    public class InvoiceImagesViewModel : ViewModelBase
    {
        public ObservableCollection<InvoiceImageModel> ImagePaths { get; set; } = 
        [
            new()
            {
                FileName = "12/02/2025",
                Url =  "https://i.pinimg.com/1200x/99/73/11/99731182addf957b8ef3ae8faf9872cb.jpg",
            },
            new()
            {
                FileName = "17/09/2025",
                Url =  "https://i.pinimg.com/736x/21/a2/52/21a2521aa1ae6338441a79e37a31df42.jpg",
            },
            new()
            {
                FileName = "22/10/2025",
                Url =   "https://i.pinimg.com/1200x/e0/20/73/e02073576c74b4a20194e5c2661f4e74.jpg",
            },
            new()
            {
                FileName = "15/09/2025",
                Url = "https://i.pinimg.com/1200x/e3/bc/a1/e3bca19586b1bdc5a5360b89b4fdc666.jpg",
            },
            new()
            {
                FileName = "17/10/2025",
                Url =  "https://i.pinimg.com/1200x/c0/41/cf/c041cf926eac3d7e37b4cfda3249dcd5.jpg"
            },
        ];
        
        private InvoiceImageModel? _selectedImage;
        public InvoiceImageModel? SelectedImage
        {
            get => _selectedImage;
            set => this.RaiseAndSetIfChanged(ref _selectedImage, value);
        }
        public bool IsDetailVisible => SelectedImage != null;
        public ReactiveCommand<InvoiceImageModel, Unit> ShowDetailCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseDetailCommand { get; }
        
        public InvoiceImagesViewModel()
        {
            ShowDetailCommand = ReactiveCommand.Create<InvoiceImageModel>(item =>
            {
                SelectedImage = item;
                this.RaisePropertyChanged(nameof(IsDetailVisible));
            });

            CloseDetailCommand = ReactiveCommand.Create(() =>
            {
                SelectedImage = null;
                this.RaisePropertyChanged(nameof(IsDetailVisible));
            });
        }
        
        public override void Dispose()
        {
            // các lệnh giải phóng tài nguyên
            base.Dispose();
        }
    }
}