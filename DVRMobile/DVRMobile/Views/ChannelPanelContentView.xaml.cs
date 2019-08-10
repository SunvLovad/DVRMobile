using DVRMobile.Base;
using DVRMobile.Services;
using DVRMobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChannelPanelContentView : ContentView
    {
        public ChannelPanelContentView()
        {
            InitializeComponent();
        }

        private ChannelPanelContentViewModel viewModel = null;
        private void UpdateGUI()
        {
            rootGrid.Children.Clear();
            rootGrid.RowDefinitions.Clear();
            rootGrid.ColumnDefinitions.Clear();

            if (Global.curDivision == ENUM_DIVISIONS.DIVISION_1)
            {
                var channelFiledGrid = new ChannelFieldGrid(viewModel.startChannelIndex, viewModel.serverIndex);
                rootGrid.Children.Add(channelFiledGrid);
            }
            else
            {
                int maxRow = GetMaxRow();
                int maxColumn = GetMaxColumn();

                for (int i = 0; i < maxRow; i++)
                {
                    var row = new RowDefinition();
                    row.Height = new GridLength(1, GridUnitType.Star);
                    rootGrid.RowDefinitions.Add(row);
                }

                for (int i = 0; i < maxColumn; i++)
                {
                    var column = new ColumnDefinition();
                    column.Width = new GridLength(1, GridUnitType.Star);
                    rootGrid.ColumnDefinitions.Add(column);
                }

                int channelIndex = 0;
                for (int r = 0; r < maxRow; r++)
                {
                    for (int c = 0; c < maxColumn; c++)
                    {
                        var channelFieldGrid = new ChannelFieldGrid(viewModel.startChannelIndex + channelIndex, viewModel.serverIndex);
                        Grid.SetRow(channelFieldGrid, r);
                        Grid.SetColumn(channelFieldGrid, c);
                        rootGrid.Children.Add(channelFieldGrid);
                        channelIndex++;
                    }
                }
            }
        }

        private int GetMaxColumn()
        {
            switch(Global.curDivision)
            {
                case ENUM_DIVISIONS.DIVISION_4:
                    return 2;
                case ENUM_DIVISIONS.DIVISION_9:
                    return 3;
                case ENUM_DIVISIONS.DIVISION_16:
                    return 4;
                case ENUM_DIVISIONS.DIVISION_36:
                    return 6;
                case ENUM_DIVISIONS.DIVISION_64:
                    return 8;
            }

            return 2;
        }

        private int GetMaxRow()
        {
            return GetMaxColumn();
        }

        private void Stop()
        {
            BindingContext = null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                var vm = BindingContext as ChannelPanelContentViewModel;
                if (vm != null)
                {
                    this.viewModel = vm;
                    viewModel.OnReloadGUI = UpdateGUI;
                    viewModel.OnStop = Stop;
                    UpdateGUI();
                }
            }
        }
    }
}