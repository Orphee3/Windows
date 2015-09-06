using System;
using Windows.Storage;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.Prism.Mvvm;
using MidiDotNet.Shared;

namespace Orphee.Views
{
    public sealed partial class HomePage : IView
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var midiFile = await KnownFolders.MusicLibrary.GetFileAsync("loop1.mid");
            MyMidiPlayer.Play(midiFile.Path);
        }

        public void Connect(int connectionId, object target)
        {
            throw new NotImplementedException();
        }
    }
}
