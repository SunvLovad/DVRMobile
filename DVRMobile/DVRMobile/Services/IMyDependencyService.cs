
using Xamarin.Forms;

namespace DVRMobile.Services
{
    public interface IMyDependencyService
    {
        void LongShowToast(string msg);

        void ShortShowToast(string msg);

        void EnterFullScreen();

        void ExitFullScreen();
    }
    public class Toast
    {
        public static void Show(string msg, Base.TOAST_LENGTH length = Base.TOAST_LENGTH.LONG)
        {
            switch (length)
            {
                case Base.TOAST_LENGTH.LONG:
                    DependencyService.Get<IMyDependencyService>().LongShowToast(msg);
                    break;
                case Base.TOAST_LENGTH.SHORT:
                    DependencyService.Get<IMyDependencyService>().ShortShowToast(msg);
                    break;
            }
        }

        public static void ShowInThread(string msg, Base.TOAST_LENGTH length = Base.TOAST_LENGTH.LONG)
        {
            Device.BeginInvokeOnMainThread(new System.Action(() =>
            {
                switch (length)
                {
                    case Base.TOAST_LENGTH.LONG:
                        DependencyService.Get<IMyDependencyService>().LongShowToast(msg);
                        break;
                    case Base.TOAST_LENGTH.SHORT:
                        DependencyService.Get<IMyDependencyService>().ShortShowToast(msg);
                        break;
                }
            }));
        }
    }

    public class FullScreen
    {
        public static void EnterFullScreen()
        {
            DependencyService.Get<IMyDependencyService>().EnterFullScreen();
        }

        public static void ExitFullScreen()
        {
            DependencyService.Get<IMyDependencyService>().ExitFullScreen();
        }
    }

}
