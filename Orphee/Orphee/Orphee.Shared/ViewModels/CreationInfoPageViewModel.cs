using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Orphee.ViewModels.Interfaces;

namespace Orphee.ViewModels
{
    public class CreationInfoPageViewModel : ViewModel, ICreationInfoPageViewModel, INavigationAware
    {
        public DelegateCommand GoBackCommand { get; private set; }
        public List<string> CommentList { get; private set; }

        public CreationInfoPageViewModel()
        {
            this.GoBackCommand = new DelegateCommand(() => App.MyNavigationService.GoBack());
            this.CommentList = new List<string>();
            InitCommentList();
        }

        public void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
        }

        private void InitCommentList()
        {
            this.CommentList.Add("Caesar Hull, DFC (1914–1940) and Paterson Hughes, DFC (1917–1940) were Royal Air Force (RAF) flying aces of the Second World War. They were killed in action in the Battle of Britain on the same day, 7 September 1940. Raised in Southern Rhodesia, South Africa and Swaziland, Hull joined No. 43 Squadron in Sussex, England, in 1935, and took part in the fighting for Narvik during the Norwegian Campaign in 1940. Hull was the RAF's first Gloster Gladiator ace and the most successful RAF pilot of the Norwegian Campaign. He later saw action as a Hawker Hurricane pilot during the Battle of Britain, in which he was killed while diving to the aid of an RAF comrade. Hughes was born and raised in Australia and took a commission with the RAF in 1937. Posted to No. 234 Squadron following the outbreak of war, he flew Supermarine Spitfires and was credited with seventeen victories during the Battle of Britain. His tally made him the highest-scoring Australian of the battle, and among the three highest-scoring Australians of the war. Hughes is generally thought to have died after his Spitfire was struck by flying debris from a German bomber that he had just shot down. (See Caesar Hull and Paterson ");
            this.CommentList.Add("Guatemalan President Otto Pérez Molina (pictured) is arrested after resigning amid charges of customs fraud.");
            this.CommentList.Add("the U.S");
            this.CommentList.Add("Wildfires across the U.S. state of Washington, including the Okanogan Complex fire, destroy more than 200 homes and burn 920 square miles (2,400 km2");
            this.CommentList.Add("Pouce bleu");
            this.CommentList.Add("Ok");
            this.CommentList.Add("Gilbert du Motier, Marquis de Lafayette Tom Simpson Hemmema");
            this.CommentList.Add("The Albanian water frog (Pelophylax shqipericus) is a species of true frog in the family Ranidae. It is native to Albania and Montenegro, where it lives in aquatic environments. The frogs are medium-sized. Males sometimes bear a distinctive bright green stripe down the length of the backbone, but otherwise are green to light brown in overall colouring with large black or dark brown spots. Females are olive green or light brown in colour and also bear brown or black large spots. The species is endangered and known populations are currently in decline.");
            this.CommentList.Add("More than 1,000,000 articles");
            this.CommentList.Add("Cool");
            this.CommentList.Add("Other areas of Wikipedia");
            this.CommentList.Add("C'est trop bien !!!");
        }
    }
}
