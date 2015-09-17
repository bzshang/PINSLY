using PIBand.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using System.Security;

using PIBand.Models;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace PIBand
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeUserPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private bool _isUserSet;

        public ChangeUserPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            tbDisplayName.Text = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetValueOrDefault("DisplayName", "");
            tbUsername.Text = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetValueOrDefault("Username", "");

            if (!string.IsNullOrEmpty(tbUsername.Text))
                tbPassword.Password = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetPassword(tbUsername.Text);

            _isUserSet = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].ContainsKey("Username");

            if (!_isUserSet)
            {
                tbTitle.Text = "Add User";
            }

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SaveUserAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].AddOrUpdateValue("DisplayName", tbDisplayName.Text);
            AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].AddOrUpdateValue("Username", tbUsername.Text);

            AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].AddOrUpdateValue("WebIds", null);

            string currentUser = AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].GetValueOrDefault("Username", "none");
            AppSettingsManager.Instance.LocalSettings.Containers["UserSettings"].AddOrUpdatePassword(currentUser, tbPassword.Password);

            _isUserSet = true;
        }

        private void BackUserAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isUserSet)
            {
                BackToSettings();
            }
            else
            {
                BackToPivot();
            } 
        }

        private void BackToSettings()
        {
            if (!Frame.Navigate(typeof(SettingsPage)))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void BackToPivot()
        {
            if (!Frame.Navigate(typeof(PivotPage)))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }
    }
}
