namespace FromRendererToCore.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new FromRendererToCore.App());
        }
    }
}
