using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DVRMobile.CustomRenderers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ToggleButton : Button
    {
        public static readonly BindableProperty NormalImagePathProperty = BindableProperty.Create(
           propertyName: "NormalImagePath",
           returnType: typeof(string),
           declaringType: typeof(ToggleButton),
           defaultValue: "",
           defaultBindingMode: BindingMode.TwoWay,
           null,
           propertyChanged: OnNormalImagePathChanged
           );

        public static readonly BindableProperty ActiveImagePathProperty = BindableProperty.Create(
           propertyName: "ActiveImagePath",
           returnType: typeof(string),
           declaringType: typeof(ToggleButton),
           defaultValue: "",
           defaultBindingMode: BindingMode.TwoWay,
           null,
           propertyChanged: OnActiveImagePathChanged
           );

        public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(
           propertyName: "IsActive",
           returnType: typeof(bool),
           declaringType: typeof(ToggleButton),
           defaultValue: false,
           defaultBindingMode: BindingMode.TwoWay,
           null,
           propertyChanged: OnIsActiveChanged
           );

        public static readonly BindableProperty IsEnableProperty = BindableProperty.Create(
           propertyName: "IsEnable",
           returnType: typeof(bool),
           declaringType: typeof(ToggleButton),
           defaultValue: true,
           defaultBindingMode: BindingMode.TwoWay,
           null,
           propertyChanged: OnIsEnableChanged
           );

        private string curImageSourcePath = string.Empty;

        public string NormalImagePath
        {
            get
            {
                return (string)GetValue(NormalImagePathProperty);
            }
            set
            {
                SetValue(NormalImagePathProperty, value);
                SetImageSource();
            }
        }

        public string ActiveImagePath
        {
            get
            {
                return (string)GetValue(ActiveImagePathProperty);
            }
            set
            {
                SetValue(ActiveImagePathProperty, value);
                SetImageSource();
            }
        }

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
                SetImageSource();
            }
        }

        public bool IsEnable
        {
            get
            {
                return (bool)GetValue(IsEnabledProperty);
            }
            set
            {
                SetValue(IsEnabledProperty, value);
                SetImageSource();
            }
        }

        private static void OnIsEnableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton tg = bindable as ToggleButton;
            if (tg != null)
            {
                tg.IsEnabled = (bool)newValue;
                tg.SetImageSource();
            }
        }

        private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton tg = bindable as ToggleButton;
            if (tg != null)
                tg.SetImageSource();
        }

        private static void OnNormalImagePathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton tg = bindable as ToggleButton;
            if (tg != null)
                tg.SetImageSource();
        }

        private static void OnActiveImagePathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton tg = bindable as ToggleButton;
            if (tg != null)
                tg.SetImageSource();
        }

        public void SetImageSource()
        {
            if(IsActive == true && IsEnable == false)
            {
                IsActive = false;
                return;
            }

            if (IsActive == true)
            {
                if (ActiveImagePath != string.Empty && curImageSourcePath != ActiveImagePath)
                {
                    this.ImageSource = ImageSource.FromFile(ActiveImagePath);
                    curImageSourcePath = ActiveImagePath;
                }
            }
            else
            {
                if (NormalImagePath != string.Empty && curImageSourcePath != NormalImagePath)
                {
                    this.ImageSource = ImageSource.FromFile(NormalImagePath);
                    curImageSourcePath = NormalImagePath;
                }
            }
        }

        public ToggleButton()
        {
            this.Clicked += ToggleButton_Clicked;
        }

        private void ToggleButton_Clicked(object sender, EventArgs e)
        {
            this.IsActive = !this.IsActive;
        }
    }
}
