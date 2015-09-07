using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Orphee.Models;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class ChatPageViewModel : ViewModel, IChatPageViewModel
    {
        public DelegateCommand BackCommand { get; private set; }
        public ObservableCollection<MyDictionary> Conversation { get; private set; }
        private Dictionary<string, string> _conversation; 

        public ChatPageViewModel()
        {
            this.BackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this._conversation = new Dictionary<string, string>();
            this.Conversation = new ObservableCollection<MyDictionary>();
            InitDictionary();
            InitConversation();
        }

        private void InitDictionary()
        {
            this._conversation.Add("Me", "Caesar Hull, DFC (1914–1940) and Paterson Hughes, DFC (1917–1940) were Royal Air Force (RAF) flying aces of the Second World War. They were killed in action in the Battle of Britain on the same day, 7 September 1940. Raised in Southern Rhodesia, South Africa and Swaziland, Hull joined No. 43 Squadron in Sussex, England, in 1935, and took part in the fighting for Narvik during the Norwegian Campaign in 1940. Hull was the RAF's first Gloster Gladiator ace and the most successful RAF pilot of the Norwegian Campaign. He later saw action as a Hawker Hurricane pilot during the Battle of Britain, in which he was killed while diving to the aid of an RAF comrade. Hughes was born and raised in Australia and took a commission with the RAF in 1937. Posted to No. 234 Squadron following the outbreak of war, he flew Supermarine Spitfires and was credited with seventeen victories during the Battle of Britain. His tally made him the highest-scoring Australian of the battle, and among the three highest-scoring Australians of the war. Hughes is generally thought to have died after his Spitfire was struck by flying debris from a German bomber that he had just shot down. (See Caesar Hull and Paterson");
            this._conversation.Add("John", "How do you like them ?");
        }

        private void InitConversation()
        {
            foreach (var message in this._conversation)
                this.Conversation.Add(new MyDictionary(message.Key == "Me" ? Visibility.Visible : Visibility.Collapsed, message.Key == "Me" ? Visibility.Collapsed : Visibility.Visible, message.Value));
        }
    }
}
