using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Models.InvoiceImages;
using ReactiveUI;

namespace BillInsight.ViewModels
{
    public class InvoiceImagesViewModel : ViewModelBase
    {
        public ObservableCollection<InvoiceImageModel> ImagePaths { get; set; } = [];
        
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